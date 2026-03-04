from proceso import Proceso, EstadoProceso


class GestorProcesos:
    def __init__(self):
        self.procesos = []
        self.contador_pid = 1

    def crear_proceso(self, tiempo_ejecucion):
        proceso = Proceso(self.contador_pid, tiempo_ejecucion)
        self.procesos.append(proceso)
        self.contador_pid += 1
        print(f"Proceso {proceso.pid} creado en estado NUEVO")

    def pasar_a_listo(self, proceso):
        if proceso.estado == EstadoProceso.NUEVO:
            proceso.estado = EstadoProceso.LISTO

    def planificar(self):
        """
        Planificación básica tipo Round Robin
        """
        for proceso in self.procesos:
            if proceso.estado == EstadoProceso.NUEVO:
                self.pasar_a_listo(proceso)

            if proceso.estado == EstadoProceso.LISTO:
                proceso.estado = EstadoProceso.EJECUTANDO
                print(f"Ejecutando proceso {proceso.pid}")
                proceso.ejecutar()

                if proceso.estado != EstadoProceso.TERMINADO:
                    proceso.estado = EstadoProceso.LISTO

    def mostrar_procesos(self):
        print("\nLista de procesos:")
        for proceso in self.procesos:
            print(proceso)