class_texts = {
	["physical"] = {
		"<color=0.7,1,0.5,1>Physical</color>: All <color=0.7,1,0.5,1>Physical</color> ships get <color=1,0.7,0.5,1>+1</color> armor",
		"Armor for <color=0.7,1,0.5,1>Physical</color> "
	},
	["energy"] = {
		"<color=0.7,1,0.5,1>Energy</color>: All ships get <color=1,0.7,0.5,1>5%</color> firing cooldown reduction",
		"Firing cooldown "
	},
	["guided"] = {
		"<color=0.7,1,0.5,1>Guided</color>: All <color=0.7,1,0.5,1>Guided</color> ships get <color=1,0.7,0.5,1>10%</color> firing cooldown reduction",
		"Firing cooldown for <color=0.7,1,0.5,1>Guided</color> "
	},
	["support"] = {
		"<color=0.7,1,0.5,1>Support</color>: All <color=0.7,1,0.5,1>Physical</color> ships get <color=1,0.7,0.5,1>+5</color> max health",
		"Max health for <color=0.7,1,0.5,1>Physical</color> "
	},
	["graviton"] = {
		"<color=0.7,1,0.5,1>Graviton</color>: All <color=0.7,1,0.5,1>Physical</color> damage is increased by <color=1,0.7,0.5,1>+10%</color>",
		"<color=0.7,1,0.5,1>Physical</color> damage increased by "
	},
	["nuker"] = {
		"<color=0.7,1,0.5,1>Nuker</color>: <color=0.7,1,0.5,1>AOE range</color> is increased by <color=1,0.7,0.5,1>+5%</color>",
		"<color=0.7,1,0.5,1>AOE range</color> range increased by "
	}
}

store_texts = {
	{
		id = 1,
		cost = 100,
		name = "Battleship",
		text = "Fires <color=1,0.7,0.5,1>3/4/5</color> cannon rounds every <color=1,0.7,0.5,1>3/2.5/2</color> seconds, dealing <color=1,0.7,0.5,1>60/80/100</color> <color=0.7,1,0.5,1>Physical</color> damage\n"..
		"<color=1,0.7,0.5,1>100/150/200</color> Health <color=1,0.7,0.5,1>5/10/15</color> Armor\n\n" ..
		"Flagship Bonus: Ships get <color=1,0.7,0.5,1>+5</color> Armor",
		classes = {"physical"}
	},
	{
		id = 2,
		cost = 150,
		name = "Laser Cutter",
		text = "Fires laser beam up to <color=1,0.7,0.5,1>500/650/800</color> meters, dealing <color=1,0.7,0.5,1>75/150/275</color> <color=0.7,1,0.5,1>Energy</color> damage/s decreasing with distance\n"..
		"<color=1,0.7,0.5,1>75/100/125</color> Health <color=1,0.7,0.5,1>2.25/2.5/2.75</color> Evasion <color=1,0.7,0.5,1>350/400/450</color> Speed\n\n" ..
		"Flagship Bonus: Firing cooldown for <color=0.7,1,0.5,1>Guided</color> reduced by <color=1,0.7,0.5,1>10%</color>",
		classes = {"energy"}
	},
	{
		id = 3,
		cost = 100,
		name = "Missile Cruiser",
		text = "Fires <color=1,0.7,0.5,1>3/4/5</color> missiles every <color=1,0.7,0.5,1>4/3.5/3</color> seconds, dealing <color=1,0.7,0.5,1>40/50/60</color> <color=0.7,1,0.5,1>Explosive</color> damage\n"..
		"<color=1,0.7,0.5,1>100/150/200</color> Health <color=1,0.7,0.5,1>5/10/15</color> Armor\n\n" ..
		"Flagship Bonus: Ships get <color=1,0.7,0.5,1>+5</color> Armor",
		classes = {"physical", "guided"}
	},
	{
		id = 4,
		cost = 120,
		name = "Minelayer",
		text = "Lays <color=1,0.7,0.5,1>2/3/4</color> mines every <color=1,0.7,0.5,1>4/3.5/3</color> seconds, dealing <color=1,0.7,0.5,1>60/100/140</color> <color=0.7,1,0.5,1>Explosive</color> damage\n"..
		"<color=1,0.7,0.5,1>100/150/200</color> Health <color=1,0.7,0.5,1>5/10/15</color> Armor\n\n" ..
		"Flagship Bonus: Ships get <color=1,0.7,0.5,1>+5</color> Armor",
		classes = {"physical"}
	},
	{
		id = 5,
		cost = 200,
		name = "Mine Healer",
		text = "Fires burst of projectiles every <color=1,0.7,0.5,1>4/3.5/3</color> seconds, dealing <color=1,0.7,0.5,1>20</color> <color=0.7,1,0.5,1>Physical</color> damage each. Projectiles have a <color=1,0.7,0.5,1>10/20/30 %</color> chance to spawn a healing mine, which heals <color=1,0.7,0.5,1>10/15/20</color> health\n"..
		"<color=1,0.7,0.5,1>75/100/125</color> Health\n\n" ..
		"Flagship Bonus: Ships get <color=1,0.7,0.5,1>+10%</color> Max health",
		classes = {"support"}
	},
	{
		id = 6,
		cost = 200,
		name = "Gravity Tether",
		text = "Applies force on asteroids in range, slowing them down. Force is multiplied by level. Deals <color=1,0.7,0.5,1>5-15</color> <color=0.7,1,0.5,1>Energy</color> damage/s, depending on distance to asteroid.\n"..
		"<color=1,0.7,0.5,1>75/100/125</color> Health\n\n" ..
		"Flagship Bonus: <color=1,0.7,0.5,1>1.75x</color> larger range",
		classes = {"graviton", "energy"}
	},
	{
		id = 7,
		cost = 200,
		name = "Graviton Trapper",
		text = "Creates an AOE with a radius of <color=1,0.7,0.5,1>160</color> meters which pulls in nearby asteroids for <color=1,0.7,0.5,1>5/7.5/10</color> seconds every <color=1,0.7,0.5,1>12</color> seconds at the position of a random asteroid.\n"..
		"<color=1,0.7,0.5,1>75/100/125</color> Health\n\n" ..
		"Flagship Bonus: Improve asteroid impact resistance by <color=1,0.7,0.5,1>10%</color>",
		classes = {"graviton"}
	},
	{
		id = 8,
		cost = 200,
		name = "Energy Nuker",
		text = "Creates an AOE with a radius of <color=1,0.7,0.5,1>160/240/320</color> meters dealing <color=1,0.7,0.5,1>100/150/200</color> damage/s for <color=1,0.7,0.5,1>5/7/9</color> seconds every <color=1,0.7,0.5,1>12</color> seconds at the position of the closest asteroid.\n"..
		"<color=1,0.7,0.5,1>75/100/125</color> Health\n\n" ..
		"Flagship Bonus: Increase all <color=0.7,1,0.5,1>Energy</color> damage by <color=1,0.7,0.5,1>25%</color>",
		classes = {"energy", "nuker"}
	},
	{
		id = 9,
		cost = 150,
		name = "Rammer",
		text = "On collision with an asteroid from the front, creates an AOE explosion with a range of <color=1,0.7,0.5,1>120/160/200</color> meters dealing <color=1,0.7,0.5,1>100/150/200</color> damage, increased by relative speed.\n"..
		"<color=1,0.7,0.5,1>150/250/350</color> Health\n" ..
		"<color=1,0.7,0.5,1>30/40/50</color> Armor\n\n" ..
		"Flagship Bonus: Ships get <color=1,0.7,0.5,1>+10</color> Armor",
		classes = {"physical"}
	}
}