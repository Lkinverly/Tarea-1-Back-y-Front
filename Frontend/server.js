const API_BASE = "http://localhost:5189";


// =====================================
const express = require("express");
const axios = require("axios");
const path = require("path");
const bodyParser = require("body-parser");

const app = express();

// EJS + estáticos
app.set("views", path.join(__dirname, "views"));
app.set("view engine", "ejs");
app.use("/public", express.static(path.join(__dirname, "public")));
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

// Helpers API
const api = axios.create({
  baseURL: API_BASE,
  
});

async function getTiposSangre() {
  const { data } = await api.get("/api/TipoSangres"); // <- endpoint de tipos de sangre
  return data;
}

// ================= RUTAS =================

// Listado
app.get("/", async (req, res) => {
  try {
    const { data } = await api.get("/api/Estudiantes");

    // ✅ Ajuste: si la respuesta viene con $values, úsalo
    const estudiantes = data.$values ? data.$values : data;

    res.render("index", { estudiantes });
  } catch (err) {
    console.error(err.message);
    res.status(500).send("Error al obtener estudiantes desde la API.");
  }
});

// Formulario: Crear
app.get("/create", async (req, res) => {
  try {
    const data = await getTiposSangre();

    // ✅ Asegurar que sea un arreglo
    const tipos = data.$values ? data.$values : data;

    res.render("create", { tipos, msg: null });
  } catch (err) {
    console.error(err.message);
    res.status(500).send("Error al obtener tipos de sangre.");
  }
});

// POST: Crear
app.post("/create", async (req, res) => {
  try {
    // El body debe coincidir con el modelo del API
    const payload = {
      carne: req.body.Carne,
      nombres: req.body.Nombres,
      apellidos: req.body.Apellidos,
      direccion: req.body.Direccion || null,
      telefono: req.body.Telefono || null,
      correo_Electronico: req.body.Correo_Electronico,
      fecha_Nacimiento: req.body.Fecha_Nacimiento, // YYYY-MM-DD
      id_Tipo_Sangre: parseInt(req.body.Id_Tipo_Sangre, 10),
    };

    await api.post("/api/Estudiantes", payload);
    res.redirect("/");
  } catch (err) {
    console.error(err.response?.data || err.message);
    const tipos = await getTiposSangre();
    res.status(400).render("create", { tipos, msg: "No se pudo crear el estudiante. Revisa los datos." });
  }
});

// Formulario: Editar
app.get("/edit/:id", async (req, res) => {
  try {
    const id = req.params.id;
    const [estResp, tiposResp] = await Promise.all([
      api.get(`/api/Estudiantes/${id}`),
      getTiposSangre(),
    ]);

    const estudiante = estResp.data;
    const tipos = tiposResp.$values ? tiposResp.$values : tiposResp;

    res.render("edit", { estudiante, tipos, msg: null });
  } catch (err) {
    console.error(err.message);
    res.status(404).send("Estudiante no encontrado.");
  }
});

// POST: Editar
app.post("/edit/:id", async (req, res) => {
  const id = req.params.id;
  try {
    const payload = {
      id_Estudiante: parseInt(id, 10),
      carne: req.body.Carne,
      nombres: req.body.Nombres,
      apellidos: req.body.Apellidos,
      direccion: req.body.Direccion || null,
      telefono: req.body.Telefono || null,
      correo_Electronico: req.body.Correo_Electronico,
      fecha_Nacimiento: req.body.Fecha_Nacimiento,
      id_Tipo_Sangre: parseInt(req.body.Id_Tipo_Sangre, 10),
    };

    await api.put(`/api/Estudiantes/${id}`, payload);
    res.redirect("/");
  } catch (err) {
    console.error(err.response?.data || err.message);
    const [estResp, tipos] = await Promise.all([
      api.get(`/api/Estudiantes/${id}`),
      getTiposSangre(),
    ]);
    res.status(400).render("edit", { estudiante: estResp.data, tipos, msg: "No se pudo actualizar el estudiante." });
  }
});

// POST: Eliminar
app.post("/delete/:id", async (req, res) => {
  try {
    await api.delete(`/api/Estudiantes/${req.params.id}`);
    res.redirect("/");
  } catch (err) {
    console.error(err.message);
    res.status(400).send("No se pudo eliminar el estudiante.");
  }
});

// Servidor
const PORT = process.env.PORT || 3000;
app.listen(PORT, () => {
  console.log(`Frontend corriendo en http://localhost:${PORT}`);
  console.log(`Usando API en: ${API_BASE}`);
});
