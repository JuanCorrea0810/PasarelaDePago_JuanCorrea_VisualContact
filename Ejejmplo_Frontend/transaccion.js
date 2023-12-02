window.document.addEventListener("DOMContentLoaded", async () => {
  const container = document.querySelector(".notification");
  const params = new URLSearchParams(window.location.search);
  const token = params.get("token");
  if (token) {
    await ConfirmarCompra(token);
  } else {
    container.classList.add("mostrar-margen");
    container.innerHTML = `<i class="fas fa-times fa-9x fallo"></i>
        <br/>
          <h4 class="fallo">Ooops!! Ocurrió un error</h4>
          <h4 class="fallo">La transacción no se pudo hacer efectiva</h4>
          <a href="index.html"><button class="regreso">Volver a la página</button></a>`;
  }
  async function ConfirmarCompra(token) {
    const response = await axios.post(
      `https://localhost:7128/api/Payment/Confirm?token=${token}`
    );
    const status = response.data.status;
    const idTransaccion = response.data.idTransaccion;
    console.log(response);
    if (status) {
      container.classList.add("mostrar-margen");

      container.innerHTML = `<i class="far fa-check-circle fa-9x exitoso"></i>
      <br/>
        <h4 class="exitoso">Compra realizada satisfactoriamente</h4>
        <h4 class="exitoso">Id de la transacción: <strong id="IdTransaccion">${idTransaccion}</strong></h4>
        <a href="index.html"><button class="regreso">Volver a la página</button></a>`;
    } else {
      container.classList.add("mostrar-margen");

      container.innerHTML = `<i class="fas fa-times fa-9x fallo"></i>
          <br/>
            <h4 class="fallo">Ooops!! Ocurrió un error</h4>
            <h4 class="fallo">La transacción no se pudo hacer efectiva</h4>
            <a href="index.html"><button class="regreso">Volver a la página</button></a>`;
    }
  }
});
