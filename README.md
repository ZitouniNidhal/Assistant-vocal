# Assistant Vocal

Assistant Vocal est un projet d'exemple C# (Windows Forms) qui simule un assistant vocal local. Il fournit une interface simple pour démarrer/arrêter une boucle d'écoute asynchrone, injecter des commandes simulées, afficher un historique, et traiter des commandes basiques (heure, ouvrir un site, répéter un texte, etc.). Le projet est conçu pour être facilement étendu avec de vrais moteurs de reconnaissance vocale et de synthèse.

## Fonctionnalités

- Interface graphique minimale (Windows Forms) avec Start/Stop et historique.
- Boucle d'écoute asynchrone annulable (CancellationToken).
- Simulation de reconnaissance vocale via une file d'attente (SpeechRecognition).
- Traitement centralisé des commandes (AssistantLogic).
- Wrapper TTS simple (TextToSpeech) avec fallback si System.Speech absent.
- Boîte de saisie pour simuler des commandes manuelles (Prompt).

## Structure du projet

- MainForm.cs — UI principale, contrôle du cycle d'écoute asynchrone, journaux et gestion des boutons.
- SpeechRecognition.cs — simulation de la file d'attente et méthode RecognizeNextAsync pour remplacer facilement par un moteur réel.
- AssistantLogic.cs — parsing et exécution des commandes vocales.
- TextToSpeech.cs — wrapper pour System.Speech.Synthesis (fallback en debug si non disponible).
- Prompt.cs — petite boîte de dialogue InputBox pour injecter des commandes.

## Commandes connues

- "quelle heure" ou toute commande contenant "heure" → retourne l'heure actuelle.
- "ouvrir <site>" → ouvre un site ou effectue une recherche Google si l'entrée n'est pas une URL.
- "répéter <texte>" → répète le texte fourni.
- "aide" → liste les commandes disponibles.
- "arrêter" / "au revoir" → arrête la boucle d'écoute et ferme l'assistant.
- "météo" → réponse indicatrice (à connecter à une API météo).

## Prérequis

- Windows (recommandé pour System.Speech)
- .NET Framework 4.x ou .NET 6/7+ (le projet peut nécessiter d'être adapté selon la cible)
- Visual Studio 2019/2022 ou dotnet SDK

Note : System.Speech.Synthesis est disponible sur .NET Framework (Windows). Si vous ciblez .NET Core/5+/6+, préférez des bibliothèques TTS cross-platform ou l'API Azure Speech.

## Compilation et exécution

1. Ouvrez la solution dans Visual Studio et restaurez les packages si nécessaire.
2. Définissez le projet comme démarrage et lancez (F5).
3. Cliquez sur "Démarrer" pour lancer la boucle d'écoute, puis utilisez "Simuler commande" pour entrer des commandes manuelles.

Ou, si le projet est un projet SDK compatible avec dotnet CLI :

1. dotnet build
2. dotnet run --project <CheminVersLeProjet>

## Extensions possibles

- Intégration d'un moteur de reconnaissance vocale (System.Speech, Vosk, Azure Speech, etc.).
- Remplacement de TTS par un moteur cloud pour une meilleure qualité.
- Ajout d'une page de configuration pour les paramètres (langue, hotword, moteur TTS).
- Persistance de l'historique et des commandes favorites.
- Actions avancées : météo via API, rappels, intégration musique, etc.

## Contribuer

Les contributions sont bienvenues : ouvrez une issue pour discuter des changements puis proposez une PR. Veuillez inclure des descriptions claires et les étapes pour reproduire / tester votre modification.

## Licence

MIT — voir le fichier LICENSE pour plus de détails.

## Contact

Pour toute question ou aide, ouvrez une issue sur le dépôt ou contactez le mainteneur.
