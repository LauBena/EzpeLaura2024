window.onload = ListadoTipoEjercicios();

function ListadoTipoEjercicios() {
    $.ajax({
        //LA URL PARA LA PETICION
        url: '../../TipoEjercicios/ListadoTipoEjercicios',
        //LA INFORMACION A ENVIAR
        //(TAMBIEN ES POSIBLE UTILIZAR UNA CADENA DE DATOS)
        data: {},
        //ESPECIFICA SI SERA PETICION POST O GET
        type: 'POST',
        //EL TIPO DE INFORMACION QUE SE ESPERA DE RESPUESTA
        dataType: 'json',
        //CODIGO A EJECUTAR SI LA PETICION ES SATISFACTORIA;
        //LA RESPUESTA ES PASADA COMO ARGUMENTO A LA FUNCION
        success: function (tipoDeEjercicios) {
            $("ModalTipoEjercicio").modal("hide");
            LimpiarModal();
            //$("#tbody-tipoejercicios").empty()
            let contenidoTabla = ``;

            $.each(tipoDeEjercicios, function (index, tipoDeEjercicio) {

                contenidoTabla += `
                <tr>
                    <td>${tipoDeEjercicio.descripcion}</td>
                    <td class="text-center">
                    <button type="button" class="btn btn-success btn-sm" onclick="AbrirModalEditar(${tipoDeEjercicio.tipoEjercicioID})">
                    <i class="fa-solid fa-marker"></i>
                    </button>
                    </td>
                    <td class="text-center">
                    <button type="button" class="btn btn-danger btn-sm" onclick="EliminarRegistro(${tipoDeEjercicio.tipoEjercicioID}">
                    <i class="fa-solid fa-trash"></i>
                    </button>
                    </td>
                </tr>
            `;
                //  $("#tbody-tipoejercicios").append(`
                //     <tr>
                //         <td>${tipoDeEjercicio.descripcion}</td>
                //         <td class="text-center"></td>
                //         <td class="text-center"></td>
                //     </tr>
                //  `);
            });

            document.getElementById("tbody-tipoejercicios").innerHTML = contenidoTabla;

        },
        //CODIGO A EJECUTAR SI FALLA LA PETICION
        //SON PASADOS COMO ARGUMENTO A LA FUNCION
        //EL OBJETO DE LA PETICION EN CRUDO Y CODIGO DE ESTATUS DE LA PETICION
        error: function (xhr, status) {
            console.log('Disculpe, hubo un problema al cargar el listado');
        }
    });
}

function LimpiarModal() {
    document.getElementById("TipoEjercicioID").value = 0;
    document.getElementById("descripcion").value = "";
}

function NuevoRegistro() {
    $("#ModalTitulo").text("Nuevo Tipo de Ejercicio");
}

function AbrirModalEditar(tipoEjercicioID) {
    $.ajax({
        //URL PARA LA PETICION
        url: '../../TipoEjercicios/ListadoTipoEjercicios',
        //LA INFORMACION A ENVIAR
        //(TAMBIEN SE PUEDE USAR UNA CADENA DE DATOS)
        data: { id: tipoEjercicioID },
        //ESPECIFICA SI ES POST O GET
        type: 'POST',
        //EL TIPO DE INFORMACION QUE SE ESPERA DE RESPUESTA
        dataType: 'json',
        //CODIGO A EJECUTAR SI LA PETICION ES SATISFACTORIA
        //LA RESPUESTA ES PASADA COMO ARGUMENTO A LA FUNCION
        success: function (tipoDeEjercicios) {
            let tipoDeEjercicio = tipoDeEjercicios[0];

            document.getElementById("TipoEjercicioID").value = tipoEjercicioID;
            $("#ModalTitulo").text("Editar Tipo de Ejercicio");
            document.getElementById("descripcion").value = tipoDeEjercicio.descripcion;
            $("#ModalTipoEjercicio").modal("show");
        },

        //CODIGO A EJECUTAR SI LA PETICION FALLA
        //SON PASADOS COMO ARGUMENTOS A LA FUNCION
        //EL OBJETODE LA PETICION EN CRUDO Y CODIGO DE ESTATUS DE LA PETICION
        error: function (xhr, status) {
            console.log('Disculpe, existio un problema al consultar el registro a ser modificado.');
        }
    });
}

function GuardarRegistro() {
    //GUARDAMOS EN UNA VARIABLE LO ESCRITO EN EL INPUT DESCRIPCION
    let tipoEjercicioID = document.getElementById("TipoEjercicioID").value;
    let descripcion = document.getElementById("descripcion").value;
    //POR UN LADO PROGRAMAR VERIFICACIONES DE DATOS EN EL FRONT CUANDO SON DE INGRESO DE VALORES Y NO ES NECESARIO VERIFICAR EN DB
    //POR OTRO LADO HACER VERIFICACIONES DE DATOS EN EL BACK, SI EXISTE EL ELEMENTO SI NECESITAMOS DB
    console.log(descripcion);
    $.ajax({
        //LA URL PARA LA PETICION
        url: '../../TipoEjercicios/GuardarTipoEjercicio',
        //LA INFORMACION A ENVIAR
        //(TAMBIEN SE PUEDEN UTILIZAR CADENAS DE DATOS)
        data: { tipoEjercicioID: tipoEjercicioID, descripcion: descripcion },
        //ESPECIFICA EL TIPO DE PETICION
        type: 'POST',
        //EL TIPO DE INFORMACION QUE SE ESPERA DE RESPUESTA
        dataType: 'json',
        //CODIGO A EJECUTAR SI LA PETICION ES SATISFACTORIA
        //LA RESPUESTA ES PASADA COMO ARGUMENTO A LA FUNCION
        success: function (resultado) {
            if (resultado != "") {
                alert(resultado);
            }
            ListadoTipoEjercicios();
        },

        //CODIGO A EJECUTAR SI LA PETICION FALLA
        //SON PASADOS COMO ARGUMENTO A LA FUNCION
        //EL OBJETO DE LA PETICION EN CRUDO Y CODIGO DE ESTATUS DE LA PETICION
        error: function (xhr, status) {
            console.log('Disculpe, existio un problema al guardar el registro');
        }
    });
}

function EliminarRegistro(tipoEjercicioID) {
    $.ajax({
        //LA URL PARA LA PETICION
        url: '../../TipoEjercicios/EliminarTipoEjercicio',
        //LA INFORMACION A ENVIAR
        //(ES POSIBLE UTILIZAR UNA CADENA DE DATOS)
        data: { tipoEjercicioID: tipoEjercicioID },
        //especifica el tipo de peticion
        type: 'POST',
        //EL TIPO DE INFORMACION QUE SE ESPERA DE RESPUESTA
        dataType: 'json',
        //CODIGO A EJECUTAR SI LA PETICION ES SATISFACTORIA
        //LA RESPUESTA SE PASA COMO ARGUMENTO DE LA FUNCION
        success: function (resultado) {
            if (!resultado) {
                alert("No se puede eliminar, existen ejercicios asociados.");
            }
            ListadoTipoEjercicios();
        },

        //CODIGO A EJECUTAR SI LA PETICION FALLA
        //SON PASADOS COMO ARGUMENTO A LA FUNCION
        //EL OBJETO DE LA PETICION EN CRUDO Y CODIGO DE ESTATUS DE LA PETICION
        error: function (xhr, status) {
            console.log('Disculpe, existio un problema al eliminar el registro');
        }
    });
}