class_texts = {
	["physical"] = {
		"<color=0.7,1,0.5,1>Physical</color>: All <color=0.7,1,0.5,1>Physical</color> ships get <color=1,0.7,0.5,1>+1</color> armor",
		"Armor "
	},
	["energy"] = {
		"<color=0.7,1,0.5,1>Energy</color>: All ships get <color=1,0.7,0.5,1>5%</color> firing cooldown reduction",
		"Firing cooldown "
	},
	["guided"] = {
		"<color=0.7,1,0.5,1>Guided</color>: All <color=0.7,1,0.5,1>Guided</color> ships get <color=1,0.7,0.5,1>10%</color> firing cooldown reduction",
		"Firing cooldown for <color=0.7,1,0.5,1>Guided</color> "
	}
}

store_texts = {
	{
		id = 1,
		cost = 100,
		name = "Battleship",
		text = "Fires <color=1,0.7,0.5,1>3/4/5</color> cannon rounds every <color=1,0.7,0.5,1>3/2.5/2</color> seconds, dealing <color=1,0.7,0.5,1>60/80/100</color> damage\n"..
		"<color=1,0.7,0.5,1>100/150/200</color> Health <color=1,0.7,0.5,1>5/10/15</color> Armor\n\n" ..
		"Flagship Bonus: Ships get <color=1,0.7,0.5,1>+5</color> Armor",
		classes = {"physical"}
	},
	{
		id = 2,
		cost = 150,
		name = "Laser Cutter",
		text = "Fires laser beam up to <color=1,0.7,0.5,1>500/650/800</color> meters, dealing <color=1,0.7,0.5,1>75/150/275</color> damage/s decreasing with distance\n"..
		"<color=1,0.7,0.5,1>75/100/125</color> Health <color=1,0.7,0.5,1>2.25/2.5/2.75</color> Evasion <color=1,0.7,0.5,1>350/400/450</color> Speed\n\n" ..
		"Flagship Bonus: Firing cooldown for <color=0.7,1,0.5,1>Guided</color> reduced by <color=1,0.7,0.5,1>10%</color>",
		classes = {"energy"}
	},
	{
		id = 3,
		cost = 100,
		name = "Missile Cruiser",
		text = "Fires <color=1,0.7,0.5,1>3/4/5</color> missiles every <color=1,0.7,0.5,1>4/3.5/3</color> seconds, dealing <color=1,0.7,0.5,1>40/50/60</color> damage\n"..
		"<color=1,0.7,0.5,1>100/150/200</color> Health <color=1,0.7,0.5,1>5/10/15</color> Armor\n\n" ..
		"Flagship Bonus: Ships get <color=1,0.7,0.5,1>+5</color> Armor",
		classes = {"physical", "guided"}
	}
}