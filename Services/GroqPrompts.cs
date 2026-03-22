namespace QueryBuilderApi.Services
{
    public static class GroqPrompts
    {
        public const string SQL_GENERATION_SYSTEM = @"Eres un experto en SQL. Tu tarea es generar queries SQL basadas en el esquema de la base de datos y la descripción del usuario.

IMPORTANTE:
- Retorna SOLO el SQL, sin explicaciones
- Si no puedes generar el SQL, retorna: ERROR: [razón]
- El SQL debe ser válido y seguro
- Usa nombres de tabla y columnas exactamente como aparecen en el esquema";

        public static string GetSqlGenerationUserPrompt(string databaseSchema, string userDescription)
        {
            return $@"Esquema de la Base de Datos:
{databaseSchema}

Solicitud del usuario:
{userDescription}

Genera el SQL query:";
        }
    }
}