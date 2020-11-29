# Week 8 + 9 - ML Agents Deel III: Jumper Exercise

### Team Jonas en ik (Pieter Ongenae & Jonas Van Haute)

### Inleiding

![](./Fotos/overview.png)

In deze tutorial zal u stapsgewijs een Unity project met <b>ML Agents</b> kunnen maken.

> Er wordt in deze tutorial ervan uit gegaan dat u een voorkennis heeft van de Unity omgeving, scripting (C#) en <b>ML Agents</b>.

In de game dat u gaat maken moet de speler over blokjes springen die op hem afkomen. Dit gaat hij zichzelf aanleren met behulp van <b>AI</b> en <b>ML Agents</b> doormiddel van positieve en negatieve beloningen. In dit project zullen volgende beloningen gebrkuikt worden.

| Omschrijving              | Beloning |
| ------------------------- | -------- |
| Aanraken van een obstakel | -1f      |
| Springen                  | -0.10f   |
| Over obstakel springen    | +0.5f    |

Om het nog een stapje moeilijker te maken zullen de blokjes uit twee richtingen komen.

### Stap 1: Project Aanmaken

![](./Fotos/AanmakenProject.png)
Als eerste moet u een nieuw Unity project aan via de Unity Hub.

> In deze tutorial is gebruik gemaakt van Unity versie 2019.4.15f1. Het is aangeraden om dezelfde versie of hoger te gebruiken om te voorkomen dat sommige funtionaliteiten niet zouden werken.

### Stap 2: ML Agents toevoegen aan project

![](./Fotos/MLAgentsInstall.png)
Eens dat het project geopend is moet u eerst <b>ML Agents</b> toevoegen aan het project. Dit gaat ervoor zorgen dat u uw speler kunt trainen om zelf over de blokjes te springen (Meer informatie over </b>ML Agents</b> kan u hier vinden: https://github.com/Unity-Technologies/ml-agents) Dit kan u doen door te navigeren naar <b>Windows</b> > <b>Package Manager</b>. Daar kan u </b>ML-Agents</b> toevoegen.

> In deze tutorial is er gebruik gemaakt van versie 1.0.5, het is aangeraden dezelfde versie te kiezen als u er zeker van wilt zijn dat alles werkt zoals in deze tutorial bedoeld is.

### Stap 3: GameObjecten aanmaken

Nu dat u </b>ML Agents</b> heeft toegevoegd kan u beginnen met stap per stap alles op te bouwen.

#### Stap 3.1: Weg aanmaken

![](./Fotos/Street.png)
Eerst moet u een weg maken waarop de speler kan staan en de blokjes kunnen spawnen en bewegen. U geeft een de weg de tag <b>Straat</b>.

#### Stap 3.2: Speler aanmaken

![](./Fotos/Player.png)
Vervolgens maakt u een speler aan (dit kan gewoon een cilinder zijn) en u plaatst deze centraal op de weg. Aan de speler moet u een <b>Rigidbody</b> en een <b>Box Collider</b> toevoegen. Bij de <b>Rigidbody</b> moet u bij <b>Freeze Position</b> X en Z aanvinken en bij <b>Freeze Rotation</b> X, Y en Z. Dit zorgt ervoor dat de <b>Player</b> geen ongewenste bewegingen kan doen bij het springen.

#### Stap 3.3: Spawnpunten aanmaken (x2)

![](./Fotos/Spawn.png)
Op de uiteindes van de weg moeten spawnpunten komen waar de blokjes waarover de speler zal moeten springen spawnen. Voor deze spawnpunten maakt u een <b>Empty GameObject</b> aan en plaats aan elke kant van de weg een spawnpunt.

#### Stap 3.5: Obstacle aanmaken (x2)

![](./Fotos/Obstacle.png)

Uit de <b>spawners</b> moeten natuurlijk obstakels komen waarover de speler moet springen. U kan een obtacle maken in een vorm naar keuze. Het is alleen belangrijk dat er net boven de <b>Obstacle</b> een doorzichtig vlak bevindt dat ervoor zal zorgen dat er bepaald kan worden wanneer de speler succesvol over een obstakel springt. U kan het vlak doorzichtig maken door de <b>Mesh Renderer</b> optie af te vinken. Aan dit doorzichtig vlak moet u de tag </b>JumpedOver</b> geven. De obstakels moeten nog niet op onze weg staan dus u maakt daar een prefab van. U moet van deze <b>Obstacle</b> twee varianten maken omdat we de obstakels uit twee verschillende <b>spawners</b> laten komen. Beide varianten hebben hun eigen tag nodig bv. <b>Obstacle1</b> en <b>Obstacle2</b>. Aan de <b>Obstacle</b> moet u een <b>Rigidbody</b> en een <b>Box Collider</b> toevoegen. Bij de Rigidbody is het heel belangrijk dat u ook aanduidt <b>Is Trigger</b>.

![](./Fotos/ObstacleInvisible.png)

#### Stap 3.6: DeathZones aanmaken

![](./Fotos/DeathZone.png)
Om te vermijden dat er te veel blokjes in de omgeving komen moet u de blokjes vernietigen wanneer ze af het speelveld gaan. Om dit te doen maakt u zowel links als rechts van de straat een plane en maakt deze invisible.

#### Stap 3.7: Score toevoegen

Om visueel de score van onze <b>Player</b> te tonen voegt u een Text - TextMeshPro toe aan de scene. Hiervoor moet u eerst TMP Essentials toevoegen, u zal hier een popup voor krijgen.

Na deze stappen zou u projectstructuur er als volgende moeten uitzien:
![](./Fotos/lijst.png)

### Stap 4: Scripts

De volgende stap is de scripts toevoegen aan de verschillende GameObjecten.

U voegt volgend script toe aan de <b>Player</b>:

Declaratie veriabelen.

```c#
    [SerializeField] private float jumpForce;
    [SerializeField] private KeyCode jumpKey;
    private bool jumpIsReady = true;
    private Rigidbody rBody;
    private Vector3 startingPosition;
    public TextMeshPro score;
    public Jumper j;
    public event Action OnReset;
```

`Initialize` - Start het initialisatieproces.

```c#
      public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        startingPosition = transform.position;
    }
```

`FixedUpdate` - Update per frame.

```c#
    private void FixedUpdate()
    {
        if (jumpIsReady)
        {
            RequestDecision();
        }

        score.text = j.GetCumulativeReward().ToString();

    }
```

`ActionReceived`

```c#
    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {
            Jump();
        }

    }
```

`OnEpisodeBegin`

```c#
    public override void OnEpisodeBegin()
    {
        Reset();
    }
```

`Heuristic`

```c#
    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;

        if (Input.GetKey(jumpKey))
        {
            actionsOut[0] = 1;
        }

    }
```

`Jump` - Zorgt ervoor dat de <b>Player</b> kan springen.

```c#
    private void Jump()
        {
            if (jumpIsReady)
            {
                rBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
                jumpIsReady = false;
                AddReward(-0.1f);
            }
        }
```

`Reset` - Reset wanneer <b>Player</b> een obstakel raakt.

```c#
    private void Reset()
    {
        jumpIsReady = true;
        transform.position = startingPosition;
        rBody.velocity = Vector3.zero;

        OnReset?.Invoke();
    }
```

`OnCollisionEnter` - Check op Jump om te vermijden dat <b>Player</b> kan springen voor dat hij de grond raakt.

```c#
    private void OnCollisionEnter(Collision collidedObj)
    {
        if (collidedObj.gameObject.CompareTag("Street"))
        {
            jumpIsReady = true;
        }
    }
```

`OnTriggerEnter` - Zorgt ervoor dat als de <b>Player</b> over een obstakel springt hij een positieve beloning krijgt en als hij tegen een obstakel botst hij een negatieve beloning krijgt.

```c#
    void OnTriggerEnter(Collider collidedObj)
    {
        if (collidedObj.gameObject.tag == "JumpedOver" || collidedObj.gameObject.tag == "JumpedOver2")
        {
            AddReward(0.5f);
        }

        if (collidedObj.gameObject.tag == "Obstacle")
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }
```

Ook moet u aan <b>Player</b> de volgende scripts uit de <b>ML Agents</b> scripts toevoegen:

- Ray Perception Sensor 3D
- Behavior Parameters

> Deze scripts bevinden zich in <b>Packages</b> > <b>ML Agents</b> > <b>Editor</b>

![](./Fotos/RayPerception.png)
![](./Fotos/Behavior.png)

Belangrijk bij de Ray perception is dat u de instellingen van het script veranderd zodat er zowel naar links als naar rechts een <b>straal</b> is zodat de speler <b>Obstacles</b> uit beide richtingen kan zien aankomen.

![](./Fotos/straal.png)

Daarna voegt u volgend script toe aan <b>spawner</b>:

Declaratie veriabelen:

```c#
    [SerializeField] private GameObject Obstacle;
    [SerializeField] private float minSpawnIntervalInSeconds;
    [SerializeField] private float maxSpawnIntervalInSeconds;

    private Jumper jumper;
    private List<GameObject> spawnedObjects = new List<GameObject>();
```

`Awake`

```c#
    private void Awake()
    {
        jumper = GameObject.Find("Player").GetComponent<Jumper>();
        jumper.OnReset += DestroyAllSpawnedObjects;

        StartCoroutine(nameof(Spawn));
    }
```

`IEnumerator`

```c#
     private IEnumerator Spawn()
    {
        var spawned = Instantiate(Obstacle, transform.position, transform.rotation, transform);
        spawnedObjects.Add(spawned);

        yield return new WaitForSeconds(Random.Range(minSpawnIntervalInSeconds, maxSpawnIntervalInSeconds));
        StartCoroutine(nameof(Spawn));
    }
```

`DestroyAllSpawnedObjects` - Obstakels vernietigen na dat de <b>Player</b> tegen een obstakel botst.

```c#
    private void DestroyAllSpawnedObjects()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            Destroy(spawnedObjects[i]);
            spawnedObjects.RemoveAt(i);
        }
    }
```

Daarna voegt u volgend script toe aan <b>Obstacle</b> variant #1:

```c#
    private Rigidbody rb;

    [SerializeField] private float minSpeed = 2;
    [SerializeField] private float maxSpeed = 15;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.left * Random.Range(minSpeed, maxSpeed);
    }
```

en volgend script aan variant #2:

```c#
    private Rigidbody rb;

    [SerializeField] private float minSpeed = 2;
    [SerializeField] private float maxSpeed = 15;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.right * Random.Range(minSpeed, maxSpeed);
    }
```

De volgorde van de scripts hangt af van de <b>spawner</b> waarop het script verbonden is. Bij het eerste script staat er `Vector3.right` en bij het tweede script `Vector3.left` het is dus de bedoeling dat u het script met `left` op de rechtse <b>spawner</b> zet en `right` op de linkse <b>spawner</b>.

en als laatste volgend script aan de Deathzone:

```c#
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Obstacle")
        {
            Destroy(col.gameObject);
        }
    }
```

Dit script zorgt ervoor dat als een obstakel tegen de DeathZone botst het obstakel vernietigd wordt.

### Stap 5: Trainen van de Player

Als de omgeving in Unity volledig is opgezet kan er begonnen worden met het trainen van de <b>Player</b>. Hiervoor wordt volgende configuratie gebruikt (YML bestand):

```c#
    behaviors:
  Jumper:
    trainer_type: ppo
    max_steps: 5e6
    time_horizon: 64
    summary_freq: 10000
    keep_checkpoints: 5
    checkpoint_interval: 50000

    hyperparameters:
      batch_size: 32
      buffer_size: 9600
      learning_rate: 3.0e-4
      learning_rate_schedule: constant
      beta: 5.0e-3
      epsilon: 0.2
      lambd: 0.95
      num_epoch: 3

    network_settings:
      num_layers: 2
      hidden_units: 128
      normalize: false
      vis_encoder_type: simple

    reward_signals:
      extrinsic:
        strength: 1.0
        gamma: 0.99
      curiosity:
        strength: 0.02
        gamma: 0.99
        encoding_size: 256
        learning_rate: 1e-3
```

Deze configuratie file moet in de Learning map van het project gezet worden (Als deze nog niet zou bestaan moet u deze eerst aanmaken).
Vervolgens moet u een console naar keuze openen (CMD, Anaconda, Miniconda etc) en moet u navigeren naar de Learning map binnen het project.

Daarna voert u volgend commando uit: `mlagents-learn Player-01.yml --run-id player-01`

De `Player-01.yml` verwijst hier naar het configuratiebestand.

Daarna moet u in Unity op play drukken en zal de <b>Player</b> beginnen trainen.
U kan de resultaten van de training zien door Tensorboard te openen met volgende commando: `tensorboard --logdir results`

U kan dan navigeren in een internetbrowser naar keuze naar localhost:6006 en daar zal u alle data vinden over de training.

![](./Fotos/Tensor.png)

## Bronvermelding

Ultimate Walkthrough for ML-Agents in Unity3D - https://towardsdatascience.com/ultimate-walkthrough-for-ml-agents-in-unity3d-5603f76f68b
