from Gestor import GestorProcesos


def mostrar_menu():
    print("\n===== GESTOR DE PROCESOS =====")
    print("1. Crear proceso")
    print("2. Ejecutar planificación")
    print("3. Mostrar procesos")
    print("4. Salir")


def main():
    gestor = GestorProcesos()

    while True:
        mostrar_menu()
        opcion = input("Seleccione una opción: ")

        if opcion == "1":
            try:
                tiempo = int(input("Ingrese el tiempo de ejecución del proceso: "))
                gestor.crear_proceso(tiempo)
            except ValueError:
                print("Por favor ingrese un número válido.")

        elif opcion == "2":
            gestor.planificar()

        elif opcion == "3":
            gestor.mostrar_procesos()

        elif opcion == "4":
            print("Saliendo del sistema...")
            break

        else:
            print("Opción inválida. Intente nuevamente.")


if __name__ == "__main__":
    main()