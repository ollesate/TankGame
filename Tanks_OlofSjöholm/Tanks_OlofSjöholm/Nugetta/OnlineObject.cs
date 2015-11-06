using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.nuggeta.game.core.ngdl.nobjects;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public interface OnlineObject
{
    JObject getJSONObject();
    Object CreateFromGameMessage(JObject obj, String ownerID);
    String getOwner();
    void setOwner(String owner);
}

abstract class OB : GameObject
{
    public String OwnerID;

    //Constructors
    public OB(Texture2D Texture, Vector2 Position, String Owner) :
        base(Texture, Position){
        
		this.OwnerID = Owner;
	}

	public OB(Texture2D Texture, Vector2 Position, float Speed, String Owner) :
        base(Texture, Position, Speed){

        this.OwnerID = Owner;
	}

	public OB(Vector2 Position, float Speed, String Owner) :
        base(Position, Speed){

        this.OwnerID = Owner;
	}

	public OB(Vector2 Position, String Owner) : 
        base(Position){

        this.OwnerID = Owner;
	}

    public OB(Vector2 Position, float Speed, Vector2 Direction, String Owner) :
        base(Position, Speed, Direction){

        this.OwnerID = Owner;
    }


    private JObject JSONObj = new JObject();

    //Lägg till de variabler du vill ha uppdaterade online
    protected void addToOnlineUpdated(String key, Object val){

        JSONObj[key] = JToken.FromObject(val);
    }

    //Kolla om det finns något att uppdatera, så att undvika skicka tomma jobjekt
    public Boolean HasOnlineUpdates() {

        return JSONObj.Count > 0;
    }

    //Hämta Jobj för att skicka online
    public JObject getOnlineUpdates() {

        JObject send = JSONObj;
        JSONObj = new JObject();

        return send;
    }

    //implementera de parametrar som kommer över nätet och skapa ett objekt
    public abstract OB CreateFromSpawningJSON(JObject obj);

    //Fyll ett jobj med de parametrar som behövs för att skapa objektet
    public abstract JObject GetSpawningJSON();

}