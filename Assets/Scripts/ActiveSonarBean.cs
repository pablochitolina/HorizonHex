using UnityEngine;

public class ActiveSonarBean
{
    private Vector2 posSonar;
    private string color;
    // Start is called before the first frame update
    public ActiveSonarBean(Vector2 newPosSonar, string newColor)
    {
        this.posSonar = newPosSonar;
        this.color = newColor;
    }

    public Vector2 getPosSonar()
    {
        return this.posSonar;
    }

    public string getColor()
    {
        return this.color;
    }
}
