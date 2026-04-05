#include <iostream>
#include <thread> //Hilos
#include <mutex> //Sincronizacion
#include <vector> //Guardar informacion
#include <chrono> //Tiempo

using namespace std;

// --- COMPONENTES DE MEMORIA ---
int RAM_DISPONIBLE = 1024; // MB de RAM totales en nuestro "sistema"
mutex mtx_ram;             // Para proteger la asignación de memoria
mutex mtx_almacenamiento;  // Para simular el cuello de botella del Disco Duro

// --- PCB EXTENDIDO (UNIDAD 1) ---
enum Estado { LISTO, EJECUTANDO, BLOQUEADO, TERMINADO };

struct PCB {
    int id;
    Estado estado;
    int memoriaRequerida; // Cuánta RAM necesita el proceso
    double tiempoEjecucion; // Para medir el rendimiento
};

// --- SIMULACIÓN DE ALMACENAMIENTO (Lento) ---
void SIMULACION(int id) {
    // Usamos un mutex porque solo un proceso puede usar el cabezal del disco a la vez
    mtx_almacenamiento.lock();
    cout << "  [DISCO] Proceso " << id << " escribiendo en almacenamiento (Operación LENTA)..." << endl;
    this_thread::sleep_for(chrono::milliseconds(2000)); // Simula latencia de disco
    mtx_almacenamiento.unlock();
}

// --- FUNCIÓN DEL PROCESO ---
void ejecutarProceso(PCB &proceso) {
    auto inicio = chrono::high_resolution_clock::now();

    // 1. GESTIÓN DE MEMORIA (Impacto en Rendimiento)
    mtx_ram.lock();
    if (RAM_DISPONIBLE >= proceso.memoriaRequerida) {
        RAM_DISPONIBLE -= proceso.memoriaRequerida;
        cout << "[SISTEMA] RAM asignada al Proceso " << proceso.id << ". RAM Restante: " << RAM_DISPONIBLE << "MB" << endl;
        mtx_ram.unlock();
    } else {
        cout << "[ERROR] Memoria insuficiente para Proceso " << proceso.id << endl;
        proceso.estado = TERMINADO;
        mtx_ram.unlock();
        return;
    }

    // 2. EJECUCIÓN (CPU)
    proceso.estado = EJECUTANDO;
    cout << "[PROCESO " << proceso.id << "] Ejecutando cálculos en CPU..." << endl;
    this_thread::sleep_for(chrono::milliseconds(500)); 

    // 3. ACCESO A ALMACENAMIENTO (Sincronización y Bloqueo)
    proceso.estado = BLOQUEADO;
    SIMULACION(proceso.id);

    // 4. LIBERACIÓN DE MEMORIA
    mtx_ram.lock();
    RAM_DISPONIBLE += proceso.memoriaRequerida;
    cout << "[SISTEMA] Proceso " << proceso.id << " finalizado. RAM liberada. RAM Actual: " << RAM_DISPONIBLE << "MB" << endl;
    mtx_ram.unlock();

    proceso.estado = TERMINADO;

    auto fin = chrono::high_resolution_clock::now();
    chrono::duration<double> duracion = fin - inicio;
    proceso.tiempoEjecucion = duracion.count();
}

int main() {
    cout << "=== GESTOR DE SISTEMAS OPERATIVOS: PROCESOS + MEMORIA + CONCURRENCIA ===" << endl;
    cout << "RAM Inicial: " << RAM_DISPONIBLE << "MB\n" << endl;

    vector<PCB> procesos = {
        {1, LISTO, 500, 0}, // Proceso que pide mucha RAM
        {2, LISTO, 200, 0},
        {3, LISTO, 400, 0}  // Este podría fallar si no hay RAM suficiente
    };

    vector<thread> hilos;

    for (int i = 0; i < procesos.size(); i++) {
        hilos.push_back(thread(ejecutarProceso, ref(procesos[i])));
    }

    for (auto &h : hilos) {
        h.join();
    }

    cout << "\n=== INFORME DE RENDIMIENTO (UNIDAD 1) ===" << endl;
    for (const auto &p : procesos) {
        cout << "Proceso " << p.id << " tardo " << p.tiempoEjecucion << " segundos (espera de disco incluida)." << endl;
    }

    return 0;
}
