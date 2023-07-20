# AdaHandle
AdaHandle.cs is a component that helps accessing the Ada-Handle API from Unity.

It implements most of the current endpoints and schemas as documented at the official API swagger: https://api.handle.me/swagger/

Please note that there may be bugs and typos in the code, and there is a lot of room for improvements. PRs are welcome.

### Dependencies
You need to install the open-source RestClient for Unity from Projecto26: https://github.com/proyecto26/RestClient
It is also available in the asset-store: https://assetstore.unity.com/packages/tools/network/rest-client-for-unity-102501

### How to use it
- Add an empty GameObject
- Add the AdaHandle.cs component to it
- Call the static methods you need from your scripts

### Implemented endpoints
- /handles => AdaHandle.Handles()
- /handles/{handle} => AdaHandle.Handle()
- /holders => AdaHandle.Holders()
- /holders/{holder_address} => AdaHandle.Holder()
- /stats => AdaHandle.Stats()

The REST calls are async and non-blocking. You can use the **AdaHandle.Busy** and **AdaHandle.Result** static properties to check if the operation has been completed and retrieve the results.
Usually this is done either with a coroutine or by providing a callback function (see example below).

When accessing the **Result** property, please remember to cast it to the actual class (schema) that the endpoint returns.
It's not a very elegant solution, but it's lightweight and works perfectly fine.

### Example

```
using System.Collections;
using UnityEngine;

public class AdaHandleExample : MonoBehaviour
{
    public string handle = "rookiez.org"; // without starting $

    void HandleCallback()
    {
        AdaHandle.HandleData handleData = AdaHandle.Result as AdaHandle.HandleData;
        Debug.Log($"Holder: {handleData.holder}");
    }

    IEnumerator HandleCoroutine()
    {
        AdaHandle.Handle(handle);
        while (AdaHandle.Busy)
        {
            yield return null;
        }

        AdaHandle.HandleData handleData = AdaHandle.Result as AdaHandle.HandleData;
        Debug.Log($"Holder: {handleData.holder}");
    }

    void Start()
    {
        // You can either do this...
        StartCoroutine(HandleCoroutine());

        // or that...
        AdaHandle.Handle(handle, HandleCallback);
    }
}
```

