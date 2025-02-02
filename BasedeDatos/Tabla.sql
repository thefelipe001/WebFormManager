CREATE TABLE FormularioPeliculas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PeliculaFavorita NVARCHAR(255) NOT NULL,
    GenerosFavoritos NVARCHAR(MAX) NULL, -- Guardaremos los géneros seleccionados separados por comas
    PlataformasStreaming NVARCHAR(MAX) NULL, -- Guardaremos las plataformas seleccionadas separadas por comas
    ActorFavorito NVARCHAR(255) NOT NULL,
    EscenaFavorita NVARCHAR(MAX) NOT NULL,
    FechaHoraUltimaPelicula DATETIME NULL
);

-- Ejemplo de inserción de datos
-- INSERT INTO FormularioPeliculas (PeliculaFavorita, GenerosFavoritos, PlataformasStreaming, ActorFavorito, EscenaFavorita, FechaHoraUltimaPelicula)
-- VALUES ('Inception', 'Acción,Drama', 'Netflix,Amazon Prime', 'Leonardo DiCaprio', 'El sueño dentro del sueño', '2025-02-02 20:30:00');
