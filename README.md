# Trabajo Práctico Integrador
## Desarrollo de Software
### Backend

## Introducción
Se desea desarrollar una plataforma de comercio electrónico (E-commerce). 
En esta primera etapa el objetivo es construir el módulo de Órdenes, permitiendo la gestión completa de éstas.

## Visión General del Producto
Del relevamiento preliminar se identificaron los siguientes requisitos:
- Los visitantes pueden consultar los productos sin necesidad de estar registrados o iniciar sesión.
- Para realizar un pedido se requiere el inicio de sesión.
- Una orden, para ser aceptada, debe incluir la información básica del cliente, envío y facturación.
- Antes de registrar la orden se debe verificar la disponibilidad de stock (o existencias) de los productos.
- Si la orden es exitosa hay que actualizar el stock de cada producto.
- Se deben poder consultar órdenes individuales o listar varias con posibilidad de filtrado.
- Será necesario el cambio de estado de una orden a medida que avanza en su ciclo de vida.
- Los administradores solo pueden gestionar los productos (alta, modificación y baja) y actualizar el estado de la orden.
- Los clientes pueden crear y consultar órdenes.

[Documento completo](https://frtutneduar.sharepoint.com/:b:/s/DSW2025/ETueAd4rTe1Gilj_Yfi64RYB5oz9s2dOamxKSfMFPREbiA?e=azZcwg) 

## Alcance para el Primer Parcial
> [!IMPORTANT]
> Del apartado `IMPLEMENTACIÓN` (Pag. 7), completo hasta el punto `6` (inclusive)


### Características de la Solución

- Lenguaje: C# 12.0
- Plataforma: .NET 8



#### Proyecto
Integrantes:

Moran Joaquin Leandro 58293 3k1
Nuñez Miguel Andres 58119 3k1
Nieva Gabriel Agustin 57792 3k1

Para configurar y ejecutar el proyecto

1. Requisitos previos

•	.NET 8 SDK instalado.
	Descárgalo desde: https://dotnet.microsoft.com/download/dotnet/8.0

•	Visual Studio 2022 actualizado (con soporte para .NET 8).

•	SQL Server (o el motor de base de datos que uses, revisa la cadena de conexión en appsettings.json).

---

2. Clonar el repositorio

Abre una terminal y ejecuta:

git clone <URL_DEL_REPOSITORIO>
cd Dsw2025Tpi

---

3. Configuración de la base de datos

	1.	Abre el archivo Dsw2025Tpi.Api\appsettings.json o abrir la solución abrir el explorador de soluciones, haciendo Ctrl+´ si no lo tiene abierto, en el buscador poner appsettings.json y hacer click en el.
	2.	Verifica o ajusta la cadena de conexión en la sección "ConnectionStrings".
	3.	Si usas Entity Framework Core, ejecuta las migraciones para crear la base de datos:

dotnet ef database update --project Dsw2025Tpi.Data

Si no tienes instalado el CLI de EF, ejecuta:
dotnet tool install --global dotnet-ef

---
4. Restaurar dependencias

Desde la raíz del proyecto, ejecuta:

dotnet restore

---
5. Compilar el proyecto

dotnet build

---
6. Ejecutar la API

Desde la carpeta Dsw2025Tpi.Api:
dotnet run

O bien, abre la solución en Visual Studio y presiona F5 para iniciar en modo depuración.

---

7. Probar la API

•	Accede a https://localhost:5001 (o el puerto configurado).
•	Si Swagger está habilitado, accede a /swagger para probar los endpoints.
•	También puedes usar Postman o herramientas similares.


Como utilizar los endpoints

1. Crear un producto:

Descripción: Crea un nuevo producto con los datos proporcionados en el
cuerpo de la solicitud.

Para usar este endpoint, hay que ir a la parte con el titulo Products, hay en el apartado POST y darle a la flechita que apunta para abajo, luego darle click en donde dice "Try it out", de ahí aparecerá un texto que entre llaves ({}), donde se puede ingresar los datos del producto. Luego de ingresar los datos, darle "Execute" y haci el proyecto mande una respuesta donde puede ser de éxito o de Error con los datos ingresados. Para dejar de utilizar el endpoint, debe darle donde dice "Cancel" y darle a la flechita que apunta arriba.

2. Obtener todos los productos:

Descripción: Retorna la lista completa de productos disponibles.

Para usar este endpoint, hay que ir a la parte con el titulo Products, hay en el apartado GET y darle a la flechita que apunta para abajo, luego darle click en donde dice "Try it out". Al darle click donde dice "Execute", aparecerán todos los productos disponibles.  Para dejar de utilizar el endpoint, debe darle donde dice "Cancel" y darle a la flechita que apunta arriba.

3. Obtener un producto por ID:

Descripción: Retorna los detalles completos de un producto específico.

Para usar este endpoint, hay que ir a la parte con el titulo Products, hay en el apartado GET, que a diferencia del primer GET, tiene escrito al costado "/api/products/{id}" y darle a la flechita que apunta para abajo, luego darle click en donde dice "Try it out". Aparecerá un campo donde se pide el id del producto que se este buscando, y al darle "Execute" (cabe recalcar que si no se pone nada en el campo id, aparecerá un mensaje de error pidiendo una id) pasaran dos cosas o que encuentra el producto y le muestre los detalles de el o que no lo encuentre, en dicho caso devolverá un Error 404.  Para dejar de utilizar el endpoint, debe darle donde dice "Cancel" y darle a la flechita que apunta arriba.

4. Actualizar un producto:

Descripción: Actualiza los datos de un producto existente con el ID proporcionado, usando los datos enviados en el cuerpo de la solicitud.

Para usar este endpoint, hay que ir a la parte con el titulo Products, hay en el apartado PUT y darle a la flechita que apunta para abajo, luego darle click en donde dice "Try it out". Aparecerá un campo donde se pide el id del producto que se este buscando y un texto que entre llaves ({}), donde se puede ingresar los datos del producto que desea cambiar. Luego darle click donde dice "Execute" ,(cabe recalcar que si no se pone nada en el campo id, aparecerá un mensaje de error pidiendo una id), y aparece el producto con los datos cambiados o que los datos no son validos o no se encuetra el producto con el id proporcionado. Para dejar de utilizar el endpoint, debe darle donde dice "Cancel" y darle a la flechita que apunta arriba.

5. Inhabilitar un producto:

Descripción: Modicar el atributo IsActive para inhabilitarlo.

Para usar este endpoint, hay que ir a la parte con el titulo Products, hay en el apartado PATCH y darle a la flechita que apunta para abajo, luego darle click en donde dice "Try it out". Aparecerá un campo donde se pide el id del producto que se este buscando y un texto que entre llaves ({}), donde se puede cambiar el true por false o vicerversa. Luego darle click donde dice "Execute" ,(cabe recalcar que si no se pone nada en el campo id, aparecerá un mensaje de error pidiendo una id), y aparecera un mensaje de que la operacion fue exitosa o un mensaje de error dicendo que no se encuetra el producto con el id proporcionado. Para dejar de utilizar el endpoint, debe darle donde dice "Cancel" y darle a la flechita que apunta arriba.

6. Eliminar un producto:

Descripcion: Borra un producto en especifico.

Para usar este endpoint, hay que ir a la parte con el titulo Products, hay en el apartado DELETE y darle a la flechita que apunta para abajo, luego darle click en donde dice "Try it out". Aparecerá un campo donde se pide el id del producto que desea eliminar, y al darle "Execute" (cabe recalcar que si no se pone nada en el campo id, aparecerá un mensaje de error pidiendo una id), y mostrara un mensaje de operacion exitosa o que no lo encuentre, en dicho caso devolverá un Error 404.  Para dejar de utilizar el endpoint, debe darle donde dice "Cancel" y darle a la flechita que apunta arriba.

7. Crear una nueva orden:

Descripción: Permite registrar una nueva orden de compra en el sistema.
La orden debe incluir un identicador de cliente (simulado), direcciones de
envío y facturación, y una lista de ítems que componen la orden
(productos con sus cantidades y precios unitarios al momento de la
compra).

Para usar este endpoint, hay que ir a la parte con el titulo Order, hay en el apartado POST y darle a la flechita que apunta para abajo, luego darle click en donde dice "Try it out", de ahí aparecerá un texto que entre llaves ({}), donde se puede ingresar los datos de la orden. Luego de ingresar los datos, darle "Execute" y haci el proyecto mande una respuesta donde puede ser de éxito o de Error con los datos ingresados. Para dejar de utilizar el endpoint, debe darle donde dice "Cancel" y darle a la flechita que apunta arriba.

8. Obtener todas las órdenes:

Descripción: Retorna una lista paginada de todas las órdenes registradas
en el sistema.

Para usar este endpoint, hay que ir a la parte con el titulo Order, hay en el apartado GET, y darle a la flechita que apunta para abajo, luego darle click en donde dice "Try it out". Aparecerá cuatro campo donde se puede filtrar por Status (Permite ltrar las órdenes por su estado (ej. Pending, Processing, Shipped, Delivered, Cancelled).), customerID (Permite filtrar las órdenes por un ID de cliente específico.), pageNumber (Número de página para paginación) y pageSize (Cantidad de elementos por página), y al darle "Execute" pasaran dos cosas o que encuentra y le muestre un array de objetos de ordenes o que no lo encuentre,en caso de alfgun fallo inesperado del servidor devolverá un Error 500.  Para dejar de utilizar el endpoint, debe darle donde dice "Cancel" y darle a la flechita que apunta arriba.

9. Obtener una orden por ID:

Descripción: Retorna los detalles completos de una orden especíca, incluyendo todos sus ítems.

Para usar este endpoint, hay que ir a la parte con el titulo Order, hay en el apartado GET, que a diferencia del primer GET, tiene escrito al costado "/api/orders/{id}" y darle a la flechita que apunta para abajo, luego darle click en donde dice "Try it out". Aparecerá un campo donde se pide el id de la orden que se este buscando, y al darle "Execute" (cabe recalcar que si no se pone nada en el campo id, aparecerá un mensaje de error pidiendo una id) pasaran dos cosas o que encuentra la orden y le muestre los detalles de el o que no lo encuentre, en dicho caso devolverá un Error 404.  Para dejar de utilizar el endpoint, debe darle donde dice "Cancel" y darle a la flechita que apunta arriba.

10. Actualizar el estado de una orden:

Descripción: Permite cambiar el estado de una orden existente. Este endpoint debe ser idempotente y solo modicar el estado de la orden.

Para usar este endpoint, hay que ir a la parte con el titulo Order, hay en el apartado PUT y darle a la flechita que apunta para abajo, luego darle click en donde dice "Try it out". Aparecerá un campo donde se pide el id de la orden que se este buscando y un texto que entre llaves ({}), donde se puede poner Pending, Processing, Shipped, Delivered, Cancelled. Luego darle click donde dice "Execute" ,(cabe recalcar que si no se pone nada en el campo id, aparecerá un mensaje de error pidiendo una id), y aparecera un mensaje de que la operacion fue exitosa o un mensaje de error (404) dicendo que no se encuetra la orden con el id proporcionado o otro error (400) que diga que el estado nuevo es invalido o la transicion de estado no es permitida. Para dejar de utilizar el endpoint, debe darle donde dice "Cancel" y darle a la flechita que apunta arriba.

