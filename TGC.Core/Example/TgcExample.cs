using System.Drawing;
using Microsoft.DirectX;
using TGC.Core.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.Sound;
using TGC.Core.Textures;
using TGC.Core.UserControls;
using TGC.Core.UserControls.Modifier;
using TGC.Core._2D;

namespace TGC.Core.Example
{
    public abstract class TgcExample
    {
        /// <summary>
        ///     Activa o desactiva el contador de frames por segundo.
        /// </summary>
        public bool FPS { get; set; }

        /// <summary>
        ///     Utilidad para visualizar los ejes cartesianos
        /// </summary>
        public TgcAxisLines AxisLines { get; set; }

        /// <summary>
        ///     Categor�a a la que pertenece el ejemplo.
        ///     Influye en donde se va a haber en el �rbol de la derecha de la pantalla.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///     Completar nombre del grupo en formato Grupo NN
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Completar con la descripci�n del TP
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Path de la carpeta Media que contiene todo el contenido visual de los ejemplos, como texturas, modelos 3D, etc.
        /// </summary>
        public string MediaDir { get; set; }

        /// <summary>
        ///     Path de la carpeta Shaders que contiene todo los shaders genericos
        /// </summary>
        public string ShadersDir { get; set; }

        /// <summary>
        ///     C�mara que esta utilizando el ejemplo
        /// </summary>
        public TgcCamera Camara { get; set; }

        /// <summary>
        ///     Utilidad para administrar las variables de usuario visibles en el panel derecho de la aplicacion.
        /// </summary>
        public TgcUserVars UserVars { get; set; }

        /// <summary>
        ///     Utilidad para crear modificadores de variables de usuario, que son mostradas en el panel derecho de la aplicacion.
        /// </summary>
        public TgcModifiers Modifiers { get; set; }

        public TgcExample(string mediaDir, string shadersDir, TgcUserVars userVars, TgcModifiers modifiers, TgcAxisLines axisLines, TgcCamera camara)
        {
            this.MediaDir = mediaDir;
            this.ShadersDir = shadersDir;
            this.UserVars = userVars;
            this.Modifiers = modifiers;
            this.AxisLines = axisLines;
            this.AxisLines.Enable = true;
            this.Camara = camara;
            this.FPS = true;

            this.Category = "Otros";
            this.Name = "Ejemplo en Blanco";
            this.Description = "Ejemplo en Blanco. Es hora de empezar a hacer tu propio ejemplo :)";
        }

        /// <summary>
        ///     Se llama cuando el ejemplo es elegido para ejecutar.
        ///     Inicializar todos los recursos y configuraciones que se van a utilizar.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Update de mi modelo
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el �ltimo frame</param>
        public abstract void Update(float elapsedTime);

        /// <summary>
        ///     Se llama para renderizar cada cuadro del ejemplo.
        /// </summary>
        /// <param name="elapsedTime">Tiempo en segundos transcurridos desde el �ltimo frame</param>
        public virtual void Render(float elapsedTime)
        {
            //Acutalizar input
            TgcD3dInput.Instance.update();

            //Actualizar la camara
            this.Camara.updateCamera(elapsedTime);
            this.Camara.updateViewMatrix(D3DDevice.Instance.Device);

            //actualizar el Frustum
            TgcFrustum.Instance.updateVolume(D3DDevice.Instance.Device.Transform.View, D3DDevice.Instance.Device.Transform.Projection);

            //limpiar texturas
            TexturesManager.Instance.clearAll();

            //actualizar Listener3D
            TgcDirectSound.Instance.updateListener3d();

            //Actualizar contador de FPS si esta activo
            if (this.FPS)
            {
                TgcDrawText.Instance.drawText("FPS: " + HighResolutionTimer.Instance.FramesPerSecond, 0, 0, Color.Yellow);
            }

            //Hay que dibujar el indicador de los ejes cartesianos
            if (this.AxisLines.Enable)
            {
                this.AxisLines.render();
            }
        }

        /// <summary>
        ///     Se llama cuando el ejemplo es cerrado.
        ///     Liberar todos los recursos utilizados.
        /// </summary>
        public virtual void Close()
        {
            D3DDevice.Instance.Device.Transform.World = Matrix.Identity;
            this.UserVars.ClearVars();
            this.Modifiers.Clear();
        }
    }
}