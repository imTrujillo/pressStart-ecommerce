# 📦 E-Commerce PressStart Full-Stack

<img src="https://pressstart-sv.vercel.app/logo.png" width="200" alt="Logo del proyecto">

## 🚀 Descripción del Proyecto

![App](https://pressstart-sv.vercel.app/screenshots/1.png)

Este proyecto es un sistema **E-Commerce completo**, compuesto por:

- **Backend en .NET 8 (Clean Architecture + DDD)**
- **Frontend en React**
- **Base de datos PostgreSQL**
- **Stripe** como sistema de pagos
- **JWT Auth** para autenticación segura

El sistema permite la gestión de productos, usuarios, pedidos, proveedores, categorías, e incorpora un flujo de compra real con checkout y webhook de Stripe.

---

## 🛠️ Tecnologías Utilizadas

### Backend (.NET 8)

![backend](https://pressstart-sv.vercel.app/screenshots/2.png)

- ASP.NET Core Web API  
- Entity Framework Core  
- Clean Architecture  
- Repository Pattern  
- PostgreSQL  
- JWT (Access + Refresh Tokens)  
- Stripe Checkout  

### Frontend

![frontend](https://pressstart-sv.vercel.app/screenshots/3.png)

- React  
- React Router  
- Axios
- Bootstrap

---

## 🧾 Proceso de Facturación en el Ecommerce

![facturacion](https://pressstart-sv.vercel.app/screenshots/4.png)

El proceso de facturación cubre todo el flujo: **usuario → pedido → pago → factura**.

---

### 1. Carrito y Pedido

1. El cliente navega por el catálogo (`/api/Product`) y agrega productos al carrito.
2. Cuando decide comprar, el frontend envía una solicitud:

  ```
  POST /api/Orders
  ```

3. Se genera un `orderId`.
4. El usuario agrega productos al pedido:

  ```
  POST /api/Orders/{orderId}/product/{productId}
  ```
    
5. También puede:
- Actualizar cantidades  
- Quitar productos  
- Consultar su pedido  

---

### 2. Proceso de Pago

Una vez confirmado el pedido:

- El cliente envía:

  ```
  POST /api/Payment/Checkout
  ```
  
- El backend:
- Calcula el monto total.
- Inicia la transacción.
- Espera el webhook de confirmación:

  ```
  POST /api/Payment/Webhook
  ```

- Si el pago es exitoso:
- El pedido pasa a estado **Pagado**
- Se genera la **Factura (Invoice)**

---

### 3. Generación de Factura

Después del pago, se registra una factura accesible desde: 

  ```
  GET /api/Invoice
  ```

La factura incluye:

- Información del usuario
- Productos comprados
- Subtotal
- Total final
- Fecha de compra
- ID de transacción

---

## 👥 Roles de Usuario

![Autenticación](https://pressstart-sv.vercel.app/screenshots/1.png)

El sistema cuenta con cuatro roles: **Invitado**, **Cliente**, **Empleado** y **Administrador**.

---

### 🟦 1. Invitado (Guest)

Usuario **no autenticado**.

#### ✔ Permisos:

- Ver productos
- Ver categorías

### 🔒 Para realizar un pedido:
Debe iniciar sesión o registrarse.

---

### 🟩 2. Cliente (Customer)

Usuario autenticado que compra productos.

#### ✔ Permisos:

- Crear pedidos
- Agregar o quitar productos del pedido  
- Ver sus pedidos
- Procesar pagos
- Ver sus facturas  
- Restablecer contraseña

### 🟧 3. Empleado (Employee)

Usuario del negocio encargado de manejar operaciones internas.

#### ✔ Permisos:

##### 📦 Inventario
- Crear, actualizar y eliminar productos
- Manejo de imágenes de productos  
- Crear y administrar categorías  
- Gestionar proveedores  

##### 🛒 Pedidos
- Ver sus pedidos  
- Actualizar estado de un pedido
- Revisar productos asociados a un pedido
 
### 🟥 4. Administrador (Admin)

Usuario con acceso total al sistema.

#### ✔ Permisos:

- Todo lo que puede hacer un Empleado
- Crear y eliminar empleados
- Administrar roles
- Ver todas las facturas
- Generar reportes financieros
- Eliminar pedidos
- Control total del inventario
- Mantenimiento de la base de datos

#### ❌ No tiene restricciones del sistema

---

## 🌐 Despliegue

- Backend → Render → https://pressstart-api.onrender.com/swagger/index.html
- Frontend → Vercel → https://pressstart-sv.vercel.app/
- Base de datos → Railway
- Pasarela de pago → Stripe
- Diccionario de datos → https://1drv.ms/x/c/5963e1d891182ff6/Eb5HJS33wbhOt22GR2P_Ae0BO6uVP7CTfzbvfY0ZWk3fbQ?e=V4MQ1j

---

## 👥 Creadores

1. Wilmer Álvarez → https://github.com/AssistedVeil86
2. César Andrade → https://github.com/c3saR-A
3. Jennyfer Cashpal → https://github.com/CharCash
4. Josué Melara → https://github.com/JosuMelara21
5. Steven Trujillo → https://github.com/imTrujillo
