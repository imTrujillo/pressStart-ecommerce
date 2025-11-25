import App from "../layouts/App";
import Guest from "../layouts/Guest";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Index as InventaryIndex } from "../pages/inventary/Index";
import { Index as ProductIndex } from "../pages/shop/products/Index";
import { Index as SuppliersIndex } from "../pages/suppliers/Index";
import { Index as ShopIndex } from "../pages/shop/Index";
import { Index as SearchIndex } from "../pages/search/Index";
import { Index as CategoriesIndex } from "../pages/categories/Index";
import { Index as OrdersIndex } from "../pages/orders/Index";
import { Index as UsersIndex } from "../pages/users/Index";
import { Index as ImageIndex } from "../pages/inventary/images/Index";
import { Index as SuccessPaymentIndex } from "../pages/orders/payment/success/Index";
import { Index as CancelPaymentIndex } from "../pages/orders/payment/cancel/Index";
import { Login } from "../pages/session/Login";
import { SignUp } from "../pages/session/Signup";
import { ForgotPassword } from "../pages/session/ForgotPassword";
import AuthProvider from "../pages/session/AuthProvider";
import PrivateRoute from "../pages/session/PrivateRoute";
import { CartProvider } from "../components/shopping/CartProvider";

export default function AppRoutes() {
  return (
    <div>
      <BrowserRouter>
        <AuthProvider>
          <CartProvider>
            <Routes>
              {/* RUTAS DEL CLIENTE */}
              <Route
                path="/"
                element={
                  <App>
                    <ShopIndex />
                  </App>
                }
              />

              <Route
                path="/producto/buscar"
                element={
                  <App>
                    <SearchIndex />
                  </App>
                }
              />

              <Route
                path="/producto/:id"
                element={
                  <App>
                    <ProductIndex />
                  </App>
                }
              />

              <Route
                path="/login"
                element={
                  <Guest>
                    <Login />
                  </Guest>
                }
              ></Route>

              <Route
                path="/sign-up"
                element={
                  <Guest>
                    <SignUp />
                  </Guest>
                }
              ></Route>

              <Route
                path="/forgotpassword"
                element={
                  <Guest>
                    <ForgotPassword />
                  </Guest>
                }
              ></Route>

              {/* RUTAS DEL ADMIN */}
              <Route element={<PrivateRoute allowedRoles={["Admin"]} />}>
                <Route
                  path="/usuarios"
                  element={
                    <App>
                      <UsersIndex />
                    </App>
                  }
                ></Route>

                <Route
                  path="/categorias"
                  element={
                    <App>
                      <CategoriesIndex />
                    </App>
                  }
                ></Route>
                <Route
                  path="/proveedores"
                  element={
                    <App>
                      <SuppliersIndex />
                    </App>
                  }
                ></Route>
              </Route>

              {/* RUTAS DEL EMPLEADO */}
              <Route
                element={<PrivateRoute allowedRoles={["User", "Admin"]} />}
              >
                <Route
                  path="/inventario"
                  element={
                    <App>
                      <InventaryIndex />
                    </App>
                  }
                ></Route>

                <Route
                  path="/inventario/:id/imgs"
                  element={
                    <App>
                      <ImageIndex />
                    </App>
                  }
                />
              </Route>

              {/* RUTAS DEL CLIENTE */}
              <Route
                element={
                  <PrivateRoute allowedRoles={["Customer", "User", "Admin"]} />
                }
              >
                <Route
                  path="/pedidos"
                  element={
                    <App>
                      <OrdersIndex />
                    </App>
                  }
                ></Route>
                <Route
                  path="/success"
                  element={
                    <App>
                      <SuccessPaymentIndex />
                    </App>
                  }
                ></Route>
                <Route
                  path="/cancel"
                  element={
                    <App>
                      <CancelPaymentIndex />
                    </App>
                  }
                ></Route>
              </Route>
            </Routes>
          </CartProvider>
        </AuthProvider>
      </BrowserRouter>
    </div>
  );
}
