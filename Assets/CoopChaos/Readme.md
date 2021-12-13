# Begriffe
## User
The user is a person active in the system.
## Player
The player is a user who has a character in the game.
# Known bugs
## Ownership
man sollte nicht im client irgendwie auf NetworkClient im NetworkManager zugreifen, 
da das immer zu sehr komischen bugs führt, da die biliothek rund um ownedobjects 
sehr buggy ist. sollte soweit wie möglich auf ownership für clients verzichten und 
nicht das ownership ändern, denn die änderungsfunktion hat einen bug, dass die falsche 
clientid zugewiesen wird (no joke) wenn du deine ownership objekte finden willst geht
das durch FindObjectsOfType<T> (oder ähnlich) und filter nach .IsOwner
## IsServer and IsClient with Resource cleanup in OnNetworkDespawn
das aufräumen von resourcen in OnNetworkDespawn sollte nicht abhängig von IsServer oder IsClient 
umgesetzt werden, da nach dem Shutdown des Servers oder Clients IsServer und IsClient sofort
auf false gesetzt wird.