export async function getProductos() {
  const response = await fetch('/api/productos');
  return response.json();
}
