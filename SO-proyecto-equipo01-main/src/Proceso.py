from enum import Enum
import time


class EstadoProceso(Enum):
    NUEVO = "NEW"
    LISTO = "READY"
    EJECUTANDO = "RUNNING"
    BLOQUEADO = "BLOCKED"
    TERMINADO = "EXIT"


class Proceso:
    def __init__(self, pid, tiempo_ejecucion):
        self.pid = pid
        self.estado = EstadoProceso.NUEVO
        self.tiempo_ejecucion = tiempo_ejecucion
        self.tiempo_restante = tiempo_ejecucion

    def __str__(self):
        return f"PID: {self.pid} | Estado: {self.estado.value} | Tiempo restante: {self.tiempo_restante}s"

    def ejecutar(self, cuanto=1):
        """
        Simula la ejecución del proceso
        """
        if self.estado != EstadoProceso.EJECUTANDO:
            return

        time.sleep(cuanto)
        self.tiempo_restante -= cuanto

        if self.tiempo_restante <= 0:
            self.estado = EstadoProceso.TERMINADO