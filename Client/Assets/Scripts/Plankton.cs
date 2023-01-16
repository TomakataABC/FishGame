using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class Plankton : MonoBehaviour
{
    public static Dictionary<ushort, Plankton> pList = new Dictionary<ushort, Plankton>();

    public ushort Id { get; private set; }

    private void OnDestroy()
    {
        pList.Remove(Id);
    }

    public static void Spawn(ushort id, Vector3 poz) {

        Plankton plankton = Instantiate(GameLogic.Singleton.PlanktonPrefab, poz, Quaternion.identity).GetComponent<Plankton>();

        plankton.Id = id;
        pList.Add(id, plankton);

    }

    [MessageHandler((ushort)ServerToClientId.planktonDie)]
    private static void KillPlankton(Message message) {
        ushort id = message.GetUShort();
        
        if (pList.TryGetValue(id, out Plankton plankton))
            Destroy(plankton.gameObject);

    }

}
