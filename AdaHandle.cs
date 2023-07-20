using System;
using UnityEngine;
using Proyecto26;
using Object = System.Object;

public class AdaHandle : MonoBehaviour
{
    public string apiUrl = "https://api.handle.me";

    static string _apiUrl;

    public static bool Busy { get; private set; }
    public static Object Result { get; private set; }

    void Awake()
    {
        _apiUrl = apiUrl;
    }


    /*
     * Endpoints
     */

    public static void Handles(string characters = "", int length = 0, string rarity = "", string numeric_modifiers = "", int records_per_page = 0, int page = -1, int slot_number = 0, string holder_address = "", Action callback = null)
    {
        Busy = true;

        string url = $"{_apiUrl}/handles";
        bool match = false;

        if (characters.Length > 0)
        {
            match = true;
            url += $"?characters={characters}";
        }

        if (length > 0)
        {
            url += (match ? "&" : "?");
            match = true;
            url += $"length={length}";
        }

        if (rarity.Length > 0)
        {
            url += (match ? "&" : "?");
            match = true;
            url += $"rarity={rarity}";
        }

        if (numeric_modifiers.Length > 0)
        {
            url += (match ? "&" : "?");
            match = true;
            url += $"numeric_modifiers={numeric_modifiers}";
        }

        if (records_per_page > 0)
        {
            url += (match ? "&" : "?");
            match = true;
            url += $"records_per_page={records_per_page}";
        }

        if (page >= 0)
        {
            url += (match ? "&" : "?");
            match = true;
            url += $"page={page}";
        }

        if (slot_number > 0)
        {
            url += (match ? "&" : "?");
            match = true;
            url += $"slot_number={slot_number}";
        }

        if (holder_address.Length > 0)
        {
            url += (match ? "&" : "?");
            url += $"holder_address={holder_address}";
        }

        RestClient.GetArray<HandleData>(url).Then(response =>
        {
            HandlesWrapper handlesWrapper = new HandlesWrapper();
            handlesWrapper.data = response;
            Result = handlesWrapper;

            Busy = false;
            callback?.Invoke();
        }).Catch(ex =>
        {
            Debug.LogWarning(ex.Message);
            Result = null;

            Busy = false;
            callback?.Invoke();
        });
    }

    public static void Handle(string handle, Action callback = null)
    {
        Busy = true;

        RestClient.Get<HandleData>($"{_apiUrl}/handles/{handle}").Then(response =>
        {
            Result = response;

            Busy = false;
            callback?.Invoke();
        }).Catch(ex =>
        {
            Debug.LogWarning(ex.Message);
            Result = null;

            Busy = false;
            callback?.Invoke();
        });
    }

    public static void Holders(int records_per_page = 0, int page = -1, Action callback = null)
    {
        Busy = true;

        string url = $"{_apiUrl}/holders";
        bool match = false;

        if (records_per_page > 0)
        {
            match = true;
            url += $"?records_per_page={records_per_page}";
        }

        if (page >= 0)
        {
            url += (match ? "&" : "?");
            url += $"page={page}";
        }

        RestClient.GetArray<HolderData>(url).Then(response =>
        {
            HoldersWrapper holdersWrapper = new HoldersWrapper();
            holdersWrapper.data = response;
            Result = holdersWrapper;

            Busy = false;
            callback?.Invoke();
        }).Catch(ex =>
        {
            Debug.LogWarning(ex.Message);
            Result = null;

            Busy = false;
            callback?.Invoke();
        });
    }

    public static void Holder(string holder_address, Action callback = null)
    {
        Busy = true;

        RestClient.Get<HolderData>($"{_apiUrl}/holders/{holder_address}").Then(response =>
        {
            Result = response;

            Busy = false;
            callback?.Invoke();
        }).Catch(ex =>
        {
            Debug.LogWarning(ex.Message);
            Result = null;

            Busy = false;
            callback?.Invoke();
        });
    }

    public static void Stats(Action callback = null)
    {
        Busy = true;

        RestClient.Get<StatsData>($"{_apiUrl}/stats").Then(response =>
        {
            Result = response;

            Busy = false;
            callback?.Invoke();
        }).Catch(ex =>
        {
            Debug.LogWarning(ex.Message);
            Result = null;

            Busy = false;
            callback?.Invoke();
        });
    }


    /*
     * Schemas
     */

    [Serializable]
    public class HandleData
    {
        public string hex;
        public string name;
        public string image;
        public string standard_image;
        public string holder;
        public int length;
        public int og_number;
        public string rarity;
        public string utxo;
        public string characters;
        public string numeric_modifiers;
        public string default_in_wallet;
        public string pfp_image;
        public string bg_image;
        public ResolvedAddresses resolved_addresses;
        public int created_slot_number;
        public int updated_slot_number;
        public bool has_datum;
        public string svg_version;
        public string image_hash;
        public string standard_image_hash;
    }

    [Serializable]
    public class HandlesWrapper
    {
        public HandleData[] data;
    }

    [Serializable]
    public class HolderData
    {
        public int total_handles;
        public string address;
        public string type;
        public string known_owner_name;
        public string default_handle;
        public bool manually_set;
    }

    [Serializable]
    public class HoldersWrapper
    {
        public HolderData[] data;
    }

    [Serializable]
    public class StatsData
    {
        public int total_handles;
        public int total_holders;
    }

    [Serializable]
    public class ResolvedAddresses
    {
        public string ada;
        /*
        public string eth;
        public string btc;
        */
    }
}
