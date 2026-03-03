from src.Proceso import Proceso
import time

def main():
    """
    Primer arranque del proyecto de Planificador de Procesos
    Inicializa los procesos y simula cambios de estado
    """
    print("=" * 50)
    print("INICIANDO PLANIFICADOR DE PROCESOS")
    print("=" * 50)
    print()  
    # Crear los procesos
    procesos = [
        Proceso(1),
        Proceso(2),
        Proceso(3)
    ]
    
    print(f"✓ Se crearon {len(procesos)} procesos")
    print()  
    # Primer arranque: recorrer los procesos y cambiar estados
    print("--- Simulando cambios de estado ---\n")
    for p in procesos:
        print(f"Proceso {p.id} está en estado: {p.estado}")
        
        # Cambiar a Running
        p.estado = "Running"
        print(f"  → Proceso {p.id} cambió a: {p.estado}")
        time.sleep(1)  # Pausa para visualizar
        
        # Volver a Ready
        p.estado = "Ready"
        print(f"  → Proceso {p.id} volvió a: {p.estado}\n")
    
    print("=" * 50)
    print("ARRANQUE COMPLETADO")
    print("=" * 50)

if __name__ == "__main__":
    main()