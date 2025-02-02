namespace WebFormManager.Models
{
    public class FormularioPeliculas
    {
        
            public int Id { get; set; }
            public string PeliculaFavorita { get; set; }
            public string GenerosFavoritos { get; set; } // Se guardarán los géneros seleccionados separados por comas
            public string PlataformasStreaming { get; set; } // Se guardarán las plataformas seleccionadas separadas por comas
            public string ActorFavorito { get; set; }
            public string EscenaFavorita { get; set; }
            public DateTime? FechaHoraUltimaPelicula { get; set; }
        

    }
}
