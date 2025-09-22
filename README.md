   *Llegar a las carpeta correspondientes *


Instala dependencias del frontend:
   ```

   npm install
   npm install concurrently open --save-dev
   ```

Instala dependencias del backend:
   ```

   dotnet restore
   ```

Ejecuta ambos proyectos (desde la carpeta `frontend`):
   ```

   npm run start:all
   ```

Esto iniciará el backend, el frontend y abrirá Swagger UI para probar las APIs.

http://localhost:5028/swagger/index.html  http://localhost:3000