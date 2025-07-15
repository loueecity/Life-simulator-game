[System.Serializable]
public class Vector3Serializable
{
    //this class is used in save persistence

    public float x, y, z;

    public Vector3Serializable(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3Serializable()
    {
    }

}
