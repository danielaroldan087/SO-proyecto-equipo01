#include <iostream>
#include <thread>   // Para crear hilos reales
#include <mutex>    // Para la sincronización (el candado)
#include <vector>   // Para guardar nuestra lista de procesos
#include <chrono>   // Para simular tiempo de espera (pausas)

using namespace proyecto;

// --- COMPONENTES DE LA UNIDAD 1: EL PCB ---
enum Estado { LISTO, EJECUTANDO, BLOQUEADO, TERMINADO };

struct PCB {
    int id;
    Estado estado;
    string nombreEstado;
};

// --- RECURSO COMPARTIDO Y SINCRONIZACIÓN ---
mutex mtx; // Este es nuestro "Semáforo" o "Candado" (Mutex)
int RecursoCompartido = 0; // Simularemos que todos los procesos quieren sumar aquí

// Función que ejecutará cada hilo (proceso)
void ejecutarProceso(PCB &proceso) {
    // 1. Cambio de estado a EJECUTANDO
    proceso.estado = EJECUTANDO;
    cout << "[PROCESO " << proceso.id << "] Estado: EJECUTANDO" << endl;

    // Simular que el proceso está haciendo algo antes de pedir el recurso
    this_thread::sleep_for(chrono::milliseconds(500));

    // 2. SINCRONIZACIÓN: Intentar entrar a la Sección Crítica
    cout << "[PROCESO " << proceso.id << "] Intentando acceder al recurso (esperando llave...)" << endl;
    
    // El mutex bloquea a los demás. Si está ocupado, el hilo se queda en "BLOQUEADO"
    mtx.lock(); 
    
    // SECCIÓN CRÍTICA (Solo un hilo entra aquí a la vez)
    proceso.estado = BLOQUEADO; // Simulamos que está "ocupado" con el recurso
    cout << "  >>> [PROCESO " << proceso.id << "] ¡OBTUVO EL RECURSO! Sumando..." << endl;
    
    int temp = RecursoCompartido;
    this_thread::sleep_for(chrono::milliseconds(1000)); // Simula que tarda procesando
    RecursoCompartido = temp + 1;
    
    cout << "  >>> [PROCESO " << proceso.id << "] Termino de usar recurso. Nuevo valor: " << RecursoCompartido << endl;
    
    mtx.unlock(); // LIBERAR el recurso para el siguiente
    // FIN DE SECCIÓN CRÍTICA

    // 3. Terminar proceso
    proceso.estado = TERMINADO;
    cout << "[PROCESO " << proceso.id << "] Estado: TERMINADO" << endl;
}

int main() {
    cout << "=== GESTOR DE PROCESOS Y CONCURRENCIA ===" << endl;

    const int NUM_PROCESOS = 3;
    vector<PCB> listaProcesos(NUM_PROCESOS);
    vector<thread> hilos; // Aquí guardamos los hilos reales

    // Crear y lanzar los procesos
    for (int i = 0; i < NUM_PROCESOS; i++) {
        listaProcesos[i].id = i + 1;
        listaProcesos[i].estado = LISTO;
        
        cout << "[SISTEMA] Creando Proceso " << listaProcesos[i].id << " en estado LISTO" << endl;

        // CONCURRENCIA REAL: Aquí se crea el hilo y empieza a correr
        hilos.push_back(thread(ejecutarProceso, ref(listaProcesos[i])));
    }

    // Esperar a que todos los hilos terminen (join)
    for (int i = 0; i < NUM_PROCESOS; i++) {
        hilos[i].join();
    }

    cout << "\n[SISTEMA] Todos los procesos han finalizado." << endl;
    cout << "[RESULTADO FINAL] El recurso compartido vale: " << RecursoCompartido << endl;

    return 0;
}
