for i in range(1, 101):
    f = open("empty" + str(i) + ".buffer", 'w')
    f.write(
        """[
    {
        "name": "position",
        "type": "float32",
        "count": 3,
        "data": []
    },
    {
        "name": "normal",
        "type": "float32",
        "count": 3,
        "data": []
    }
]""")
    f.close()
#for i in range(1, 101):
#    print("""go.property("buffer{}", resource.buffer("/game_objects/asteroids/buffers/empty{}.buffer")) """.format(i, i))
