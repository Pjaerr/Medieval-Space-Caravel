using System.Collections;
using UnityEngine;

public class Level
{
	public LandMass[] landMass;

	/*Should contain, in order.
	
		Vector2: Left Boundary positon.
		Vector2: Left Boundary localScale.
				Top
				Top
				Right..
				Bottom ..
	 */
	public Vector2[] levelBoundaryData;
	/*Should contain, in order: Enemy spawn point positions and then player spawn point at the very end.* */
	public Vector2[] spawnPointData;

	//Generates the level using its exisiting objects and data.
	public void create()
	{
		/*Should replace the default level boundaries data inside of levelGen with the data stored in levelBoundaryData[]
		When level gen has moved the boundaries and placed the water tiles. It should loop through the LandMass array
		and instantiate LandMass[i].prefab at LandMass[i].positon*/
	}

	public Level(Vector2[] levelBoundaryData, Vector2[] spawnPointData, LandMass[] landMass)
	{
		this.levelBoundaryData = levelBoundaryData;
		this.spawnPointData = spawnPointData;
		this.landMass = landMass;
	}
}
