namespace SmartAssembly.DatabaseSample.Data
{
    //Numbers represent the value stored in the database. Their particular values must stay the same for cross-version compatbility - as database integers are cast to flags directly.
	public enum Flag
	{
        None = 0,
        Red = 10,
        Blue = 20,
        Yellow = 30,
        Green = 40,
        Orange = 50,
        Purple = 60,
        Complete = 100
	}
}
