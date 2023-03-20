using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;
using System.Security.Cryptography.X509Certificates;

namespace negocio
{
    public class PokemonNegocio
    {

        public List<Pokemon> listar(string id = "")
        {
            List<Pokemon> lista = new List<Pokemon>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security= true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion debilidad, P.IdTipo, P.IdDebilidad, P.Id, P.Activo From POKEMONS P, ELEMENTOS E, ELEMENTOS D  Where E.Id = P.IdTipo  And D.Id = P.IdDebilidad  ";

                if (id != "")
                    comando.CommandText += " and P.Id = " + id;

                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)lector["Id"];
                    aux.Numero = lector.GetInt32(0);
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    if (!(lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)lector["UrlImagen"];

                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];
                    aux.Activo = bool.Parse(lector["Activo"].ToString());

                    lista.Add(aux);

                }

                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public List<Pokemon> listarConSp()
        {
            {
                List<Pokemon> lista = new List<Pokemon>();
                AccesoDatos datos = new AccesoDatos();
                try
                {
                    //string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion debilidad, P.IdTipo, P.IdDebilidad, P.Id From POKEMONS P, ELEMENTOS E, ELEMENTOS D  Where E.Id = P.IdTipo  And D.Id = P.IdDebilidad and P.Activo = 1 ";                    
                    //datos.setearConsulta(consulta);

                    datos.setearProcedimiento("storedListar");

                    datos.ejecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Pokemon aux = new Pokemon();
                        aux.Id = (int)datos.Lector["Id"];
                        aux.Numero = datos.Lector.GetInt32(0);
                        aux.Nombre = (string)datos.Lector["Nombre"];
                        aux.Descripcion = (string)datos.Lector["Descripcion"];
                        if (!(datos.Lector["UrlImagen"] is DBNull))
                            aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                        aux.Tipo = new Elemento();
                        aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                        aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                        aux.Debilidad = new Elemento();
                        aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                        aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];
                        aux.Activo = bool.Parse(datos.Lector["Activo"].ToString());

                        lista.Add(aux);

                    }

                    return lista;
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }


        public void agregar(Pokemon nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("insert into POKEMONS (Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen) values (" + nuevo.Numero + " ,'" + nuevo.Nombre + "','" + nuevo.Descripcion + "',1, @IdTipo, @IdDebilidad,@UrlImagen)");
                datos.setearParametro("@IdTipo", nuevo.Tipo.Id);
                datos.setearParametro("@IdDebilidad", nuevo.Debilidad.Id);
                datos.setearParametro("@UrlImagen", nuevo.UrlImagen);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally
            {
                datos.cerrarConexion();
            }
        }

        public void agregarConSP(Pokemon nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("storedAltaPokemon");
                datos.setearParametro("@numero", nuevo.Numero);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@desc", nuevo.Descripcion);
                datos.setearParametro("@img", nuevo.UrlImagen);
                datos.setearParametro("@idTipo", nuevo.Tipo.Id);
                datos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);
                //datos.setearParametro("@idEvolucion", null);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Pokemon poke)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update POKEMONS set Numero = @Numero, Nombre = @Nombre, Descripcion = @Descripcion, UrlImagen = @UrlImagen, IdTipo = @IdTipo, IdDebilidad = @IdDebilidad where Id = @Id  ");
                datos.setearParametro("@Numero", poke.Numero);
                datos.setearParametro("@Nombre", poke.Nombre);
                datos.setearParametro("@Descripcion", poke.Descripcion);
                datos.setearParametro("@UrlImagen", poke.UrlImagen);
                datos.setearParametro("@IdTipo", poke.Tipo.Id);
                datos.setearParametro("@IdDebilidad", poke.Debilidad.Id);
                datos.setearParametro("Id", poke.Id);

                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificarConSP(Pokemon nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("storedModificarPokemon");
                datos.setearParametro("@numero", nuevo.Numero);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@desc", nuevo.Descripcion);
                datos.setearParametro("@img", nuevo.UrlImagen);
                datos.setearParametro("@idTipo", nuevo.Tipo.Id);
                datos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);
                datos.setearParametro("@id", nuevo.Id);

                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Pokemon> filtrar(string campo, string criterio, string filtro, string estado)
        {
            List<Pokemon> lista = new List<Pokemon>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select Numero, Nombre, P.Descripcion, UrlImagen, E.Descripcion Tipo, D.Descripcion debilidad, P.IdTipo, P.IdDebilidad, P.Id, P.Activo From POKEMONS P, ELEMENTOS E, ELEMENTOS D  Where E.Id = P.IdTipo  And D.Id = P.IdDebilidad And ";
                if (campo == "Número")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Numero > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Numero < " + filtro;
                            break;
                        default:
                            consulta += "Numero = " + filtro;
                            break;
                    }
                }
                else if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "' ";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%' ";
                            break;
                    }
                }

                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "E.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "E.Descripcion like '%" + filtro + "' ";
                            break;
                        default:
                            consulta += "E.Descripcion like '%" + filtro + "%' ";
                            break;
                    }
                }

                if (estado == "activo")               
                    consulta += " and P.Activo = 1";            
                else if (estado == "inactivo")              
                    consulta += " and P.Activo = 0";              
   
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];

                    aux.Tipo = new Elemento();
                    aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];
                    aux.Activo = bool.Parse(datos.Lector["Activo"].ToString());

                    lista.Add(aux);

                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from POKEMONS where id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void eliminarLogico(int id, bool activo = false)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("update POKEMONS set Activo = @activo where id = @id");
                datos.setearParametro("@id", id);
                datos.setearParametro("@activo", activo);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
