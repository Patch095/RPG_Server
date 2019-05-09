using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1) Collegare 2 giocatori tramite server;
//      - Il Server quando abbinera i player li abbinera' i tag BlueTeam, RedTeam
//      - Prima di entrare in game i giocatori potranno scegliere il party

// 2) Inizio del Game
//      - Init di modeli, gameLogic Scripts, UI ...

// 3) Game
//      - Il gioco presenta 3 tipi FSM:
//          - CharacterStateMachine, ogni personaggio ha la sua, gestisce ATB e le animazioni
//          - BattleStateMachine, gestisce l'ordine delle azioni eseguite, comunichera' con il server
//          - UIManager, gestisce gli input dei giocatari tramite UI
//      - Quando un personaggio e' pronto il giocatore potra' eseguire una azione
//      - Scelto l'azione il client inviera al server un pacchetto "Turno"
//      - Pacchetto Turno :
//          - Attaccante
//          - Attacco
//          - Bersagli
//      - Il Server ricevera' il pacchetto "Turno", e lo inviera' ad entrambi i client
//      - Quando entrambi i client riceveranno il Pacchetto la azione avvera' sui di loro