Tarea a realizar

1. Base de datos
La base de datos usada es SQL Server 2022. Para usarla, es necesario setear el string de conexion en el archivo GeoPagos\appsettings.json y crear la base manualmente con el nombre TennisGame.
Hay datos semilla que se insertan en la base al ejecutar el programa. Se crean los siguientes jugadores
  {
    "id": "11111111-1111-1111-1111-111111111111",
    "name": "Florencia",
    "skill": 85,
    "gender": 1
  },
  {
    "id": "22222222-2222-2222-2222-222222222222",
    "name": "Milagros",
    "skill": 90,
    "gender": 1
  },
  {
    "id": "33333333-3333-3333-3333-333333333333",
    "name": "Juan",
    "skill": 85,
    "gender": 0
  },
  {
    "id": "44444444-4444-4444-4444-444444444444",
    "name": "Joaquin",
    "skill": 90,
    "gender": 0

y se crea un torneo con las jugadoras Florencia y Milagros, con el ID "55555555-5555-5555-5555-555555555555".
Esto se hizo para que sea sencillo probar el funcionamiento de un caso simple de torneo entre dos jugadores.

2. Endpoints
   a. GET /Player obtiene todos los jugadores, con los datos base de un jugador (sea masculino o femenino).
   b. POST /Player crea un jugador. Name, Skill y Gender son obligatorios, y dependiendo del genero del jugador, las otras habilidades pueden o no ser obligatorias.
   Gender 0 (male) implica que deben enviarse unicamente los valores de Speed y Strength.
   Gender 1 (female) implica que debe enviarse unicamente el valor de ReactionTime.
     {
      "name": "string",
      "skill": 0,
      "gender": 0,
      "reactionTime": 0,
      "strength": 0,
      "speed": 0
    }

   c. GET /Tournament obtiene todos los torneos, y se puede filtrar por tipo (genero), fecha desde y hasta, y si estan finalizados.
   d. POST /Tournament crea un torneo, inscribiendo la lista de jugadores.
     {
    "type": 0,
    "players": [
          "33333333-3333-3333-3333-333333333333",
         "44444444-4444-4444-4444-444444444444"
      ]
    }
   e. PUT /Tournament/Start/{tournamentId} inicia un torneo, segun el id de torneo enviado. Devuelve el ganador del torneo.
