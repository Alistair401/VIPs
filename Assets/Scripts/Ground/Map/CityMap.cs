using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ground.Map
{
    public class CityMap
    {
        private readonly Grid _grid;

        private readonly Tile[] _tiles;
        private List<District> _districts;
        private List<Block> _blocks;
        private List<Room> _rooms;


        public CityMap(Grid grid)
        {
            _grid = grid;
            _tiles = new Tile[grid.ActualXLength * grid.ActualYLength];

            for (int i = 0; i < grid.ActualXLength; i++)
            {
                for (int j = 0; j < grid.ActualYLength; j++)
                {
                    _tiles[i + j * grid.ActualXLength] = new Tile
                    {
                        Type = 0,
                        Position = grid.GetNodes()[i + j * grid.ActualXLength],
                        ArrayX = i,
                        ArrayY = j
                    };
                }
            }
            GenerateDistricts();
            GenerateBlocks();
            GenerateRooms();
            ConnectRooms();
        }

        private void GenerateDistricts()
        {
            int minSize = 12;

            _districts = new List<District>
            {
                new District
                {
                    MaxX = _grid.ActualXLength,
                    MaxY = _grid.ActualYLength,
                    MinX = 0,
                    MinY = 0
                }
            };

            int roadCount = 0;
            float likelihood = 0.5f;
            while (roadCount < 25)
            {
                District currentDistrict = _districts[0];
                int totalSize = _districts.Sum(district => district.Probability);

                int diceRoll = Random.Range(0, totalSize);
                int cumProbability = 0;
                foreach (District district in _districts)
                {
                    cumProbability += district.Probability;
                    if (cumProbability <= diceRoll) continue;
                    currentDistrict = district;
                    break;
                }

                if (Random.value > likelihood)
                {
                    if (currentDistrict.MaxX - currentDistrict.MinX < minSize)
                    {
                        continue;
                    }
                    int splitIndex = Random.Range(
                        currentDistrict.MinX + minSize / 2,
                        currentDistrict.MaxX - minSize / 2
                    );
                    _districts.Add(new District
                    {
                        MinX = currentDistrict.MinX,
                        MinY = currentDistrict.MinY,
                        MaxX = splitIndex - 1,
                        MaxY = currentDistrict.MaxY
                    });
                    _districts.Add(new District
                    {
                        MinX = splitIndex + 1,
                        MinY = currentDistrict.MinY,
                        MaxX = currentDistrict.MaxX,
                        MaxY = currentDistrict.MaxY
                    });
                    if (splitIndex < 1)
                    {
                        continue;
                    }
                    for (int j = currentDistrict.MinY; j < currentDistrict.MaxY; j++)
                    {
                        _tiles[splitIndex - 1 + j * _grid.ActualXLength].Type = Tile.Road;
                        _tiles[splitIndex + j * _grid.ActualXLength].Type = Tile.Road;
                    }
                    _districts.Remove(currentDistrict);
                    likelihood += 0.3f;
                }
                else
                {
                    if (currentDistrict.MaxY - currentDistrict.MinY < minSize)
                    {
                        continue;
                    }
                    int splitIndex = Random.Range(
                        currentDistrict.MinY + minSize / 2,
                        currentDistrict.MaxY - minSize / 2
                    );
                    _districts.Add(new District
                    {
                        MinX = currentDistrict.MinX,
                        MinY = currentDistrict.MinY,
                        MaxX = currentDistrict.MaxX,
                        MaxY = splitIndex - 1
                    });
                    _districts.Add(new District
                    {
                        MinX = currentDistrict.MinX,
                        MinY = splitIndex + 1,
                        MaxX = currentDistrict.MaxX,
                        MaxY = currentDistrict.MaxY
                    });
                    if (splitIndex < 1)
                    {
                        continue;
                    }
                    for (int i = currentDistrict.MinX; i < currentDistrict.MaxX; i++)
                    {
                        _tiles[i + (splitIndex - 1) * _grid.ActualXLength].Type = Tile.Road;
                        _tiles[i + splitIndex * _grid.ActualXLength].Type = Tile.Road;
                    }
                    _districts.Remove(currentDistrict);
                    likelihood -= 0.3f;
                }
                roadCount++;
            }
        }

        private void GenerateBlocks()
        {
            int minSize = 8;
            _blocks = new List<Block>();

            foreach (District district in _districts)
            {
                _blocks.Add(new Block
                {
                    MinX = district.MinX,
                    MinY = district.MinY,
                    MaxX = district.MaxX,
                    MaxY = district.MaxY
                });
            }

            float likelihood = 0.5f;

            for (int iterations = 0; iterations < 100; iterations++)
            {
                Block currentBlock = _blocks[0];
                int totalSize = _blocks.Sum(block => block.Probability);

                int diceRoll = Random.Range(0, totalSize);
                int cumProbability = 0;
                foreach (Block block in _blocks)
                {
                    cumProbability += block.Probability;
                    if (cumProbability <= diceRoll) continue;
                    currentBlock = block;
                    break;
                }

                if (Random.value > likelihood)
                {
                    if (currentBlock.MaxX - currentBlock.MinX < minSize)
                    {
                        continue;
                    }
                    int splitIndex = Random.Range(currentBlock.MinX + minSize / 2, currentBlock.MaxX - minSize / 2);
                    _blocks.Add(new Block
                    {
                        MinX = currentBlock.MinX,
                        MinY = currentBlock.MinY,
                        MaxX = splitIndex,
                        MaxY = currentBlock.MaxY
                    });
                    _blocks.Add(new Block
                    {
                        MinX = splitIndex + 1,
                        MinY = currentBlock.MinY,
                        MaxX = currentBlock.MaxX,
                        MaxY = currentBlock.MaxY
                    });
                    if (splitIndex < 1)
                    {
                        continue;
                    }
                    for (int j = currentBlock.MinY; j < currentBlock.MaxY; j++)
                    {
                        _tiles[splitIndex + j * _grid.ActualXLength].Type = Tile.Road;
                    }
                    likelihood += 0.3f;
                }
                else
                {
                    if (currentBlock.MaxY - currentBlock.MinY < minSize)
                    {
                        continue;
                    }
                    int splitIndex = Random.Range(currentBlock.MinY + minSize / 2, currentBlock.MaxY - minSize / 2);
                    _blocks.Add(new Block
                    {
                        MinX = currentBlock.MinX,
                        MinY = currentBlock.MinY,
                        MaxX = currentBlock.MaxX,
                        MaxY = splitIndex
                    });
                    _blocks.Add(new Block
                    {
                        MinX = currentBlock.MinX,
                        MinY = splitIndex + 1,
                        MaxX = currentBlock.MaxX,
                        MaxY = currentBlock.MaxY
                    });
                    if (splitIndex < 1)
                    {
                        continue;
                    }
                    for (int i = currentBlock.MinX; i < currentBlock.MaxX; i++)
                    {
                        _tiles[i + splitIndex * _grid.ActualXLength].Type = Tile.Road;
                    }
                    likelihood -= 0.3f;
                }
                _blocks.Remove(currentBlock);
            }
        }

        private void GenerateRooms()
        {
            int maxSplits = 4;
            _rooms = new List<Room>();
            foreach (Block block in _blocks)
            {
                if (block.Size < 64)
                {
                    _rooms.Add(new Room
                    {
                        MinX = block.MinX,
                        MinY = block.MinY,
                        MaxX = block.MaxX,
                        MaxY = block.MaxY
                    });
                    continue;
                }
                int numSplits = 0;
                int cumProbability = 0;
                int totalProbability = 0;
                for (int i = 1; i < maxSplits; i++)
                {
                    totalProbability += block.Probability;
                }
                int diceRoll = Random.Range(1, totalProbability);
                for (int i = 1; i < maxSplits; i++)
                {
                    cumProbability += block.Probability;
                    if (cumProbability > diceRoll)
                    {
                        numSplits = i;
                        break;
                    }
                }
                if (block.XLength > block.YLength * 2)
                {
                    int splitLength = block.XLength / (numSplits + 1);
                    int minX = block.MinX;
                    for (int splitIteration = 0; splitIteration < numSplits; splitIteration++)
                    {
                        _rooms.Add(new Room
                        {
                            MinX = minX,
                            MaxX = minX + splitLength,
                            MinY = block.MinY,
                            MaxY = block.MaxY
                        });
                        minX += splitLength;
                    }
                    _rooms.Add(new Room
                    {
                        MinX = minX,
                        MaxX = block.MaxX,
                        MinY = block.MinY,
                        MaxY = block.MaxY
                    });
                }
                else if (block.YLength > block.XLength * 2)
                {
                    int splitLength = block.YLength / (numSplits + 1);
                    int minY = block.MinY;
                    for (int splitIteration = 0; splitIteration < numSplits; splitIteration++)
                    {
                        _rooms.Add(new Room
                        {
                            MinX = block.MinX,
                            MaxX = block.MaxX,
                            MinY = minY,
                            MaxY = minY + splitLength
                        });
                        minY += splitLength;
                    }
                    _rooms.Add(new Room
                    {
                        MinX = block.MinX,
                        MaxX = block.MaxX,
                        MinY = minY,
                        MaxY = block.MaxY
                    });
                }
                else
                {
                    int splitX = block.XLength / 2;
                    int splitY = block.YLength / 2;
                    _rooms.Add(new Room
                    {
                        MinX = block.MinX,
                        MaxX = block.MinX + splitX,
                        MinY = block.MinY,
                        MaxY = block.MinY + splitY
                    });
                    _rooms.Add(new Room
                    {
                        MinX = block.MinX,
                        MaxX = block.MinX + splitX,
                        MinY = block.MinY + splitY,
                        MaxY = block.MaxY
                    });
                    _rooms.Add(new Room
                    {
                        MinX = block.MinX + splitX,
                        MaxX = block.MaxX,
                        MinY = block.MinY + splitY,
                        MaxY = block.MaxY
                    });
                    _rooms.Add(new Room
                    {
                        MinX = block.MinX + splitX,
                        MaxX = block.MaxX,
                        MinY = block.MinY,
                        MaxY = block.MinY + splitY
                    });
                }
            }
            foreach (Room room in _rooms)
            {
                for (int i = room.MinX; i < room.MaxX; i++)
                {
                    for (int j = room.MinY; j < room.MaxY; j++)
                    {
                        Tile tile = GetTile(i, j);
                        tile.Type = Tile.Room;
                        tile.OwningRoom = room;
                    }
                }
                for (int i = room.MinX + 1; i < room.MaxX - 1; i++)
                {
                    if (IsPossibleDoor(i, room.MinY))
                    {
                        room.AddPossibleDoor(GetTile(i, room.MinY));
                    }
                    GetTile(i, room.MinY).IsWall = true;
                    GetTile(i, room.MinY).Facing = Vector2.up;
                    if (IsPossibleDoor(i, room.MaxY - 1))
                    {
                        room.AddPossibleDoor(_tiles[i + (room.MaxY - 1) * _grid.ActualXLength]);
                    }
                    GetTile(i, room.MaxY - 1).IsWall = true;
                    GetTile(i, room.MaxY - 1).Facing = Vector2.down;
                }
                for (int j = room.MinY + 1; j < room.MaxY - 1; j++)
                {
                    if (IsPossibleDoor(room.MinX, j))
                    {
                        room.AddPossibleDoor(_tiles[room.MinX + j * _grid.ActualXLength]);
                    }
                    GetTile(room.MinX, j).IsWall = true;
                    GetTile(room.MinX, j).Facing = Vector2.left;
                    if (IsPossibleDoor(room.MaxX - 1, j))
                    {
                        room.AddPossibleDoor(_tiles[room.MaxX - 1 + j * _grid.ActualXLength]);
                    }
                    GetTile(room.MaxX - 1, j).IsWall = true;
                    GetTile(room.MaxX - 1, j).Facing = Vector2.right;
                }
                GetTile(room.MinX, room.MinY).IsWall = true;
                GetTile(room.MinX, room.MinY).IsCorner = true;
                GetTile(room.MinX, room.MinY).Facing = Vector3.up;
                GetTile(room.MinX, room.MaxY - 1).IsWall = true;
                GetTile(room.MinX, room.MaxY - 1).IsCorner = true;
                GetTile(room.MinX, room.MaxY - 1).Facing = Vector3.left;
                GetTile(room.MaxX - 1, room.MinY).IsWall = true;
                GetTile(room.MaxX - 1, room.MinY).IsCorner = true;
                GetTile(room.MaxX - 1, room.MinY).Facing = Vector3.right;
                GetTile(room.MaxX - 1, room.MaxY - 1).IsWall = true;
                GetTile(room.MaxX - 1, room.MaxY - 1).IsCorner = true;
                GetTile(room.MaxX - 1, room.MaxY - 1).Facing = Vector3.down;
            }
        }

        private void ConnectRooms()
        {
            List<Room> unconnectedRooms = new List<Room>();
            foreach (Room room in _rooms)
            {
                unconnectedRooms.Add(room);
            }

            Tile startTile = _tiles[0];
            foreach (Tile tile in _tiles)
            {
                if (tile.Type == Tile.Road)
                {
                    startTile = tile;
                    break;
                }
            }

            Queue<Tile> tilesToVisit = new Queue<Tile>();
            tilesToVisit.Enqueue(startTile);

            while (tilesToVisit.Count > 0)
            {
                Tile currentTile = tilesToVisit.Dequeue();
                if (currentTile.Visited)
                {
                    continue;
                }
                currentTile.Visited = true;


                foreach (Tile tile in GetAdjacentTiles(currentTile.ArrayX, currentTile.ArrayY))
                {
                    if (tile == null)
                    {
                        continue;
                    }
                    if (tile.Type == Tile.Room)
                    {
                        int roomIndex = unconnectedRooms.IndexOf(tile.OwningRoom);
                        if (roomIndex != -1 && unconnectedRooms[roomIndex].GetPossibleDoors().Contains(tile))
                        {
                            tile.Type = Tile.Door;
                            unconnectedRooms.Remove(tile.OwningRoom);
                        }
                    }
                    if (!tile.Visited && tile.Type == Tile.Road)
                    {
                        tilesToVisit.Enqueue(tile);
                    }
                }
            }
        }

        private Tile[] GetAdjacentTiles(int x, int y)
        {
            Tile topTile = GetTile(x, y + 1);
            Tile bottomTile = GetTile(x, y - 1);
            Tile leftTile = GetTile(x - 1, y);
            Tile rightTile = GetTile(x + 1, y);

            return new[]
            {
                topTile,
                bottomTile,
                leftTile,
                rightTile
            };
        }

        private bool IsPossibleDoor(int x, int y)
        {
            if (_tiles[x + y * _grid.ActualXLength].Type != Tile.Room) return false;
            bool adjacentToRoom = false;
            bool adjacentToRoad = false;
            for (int i = -1; i <= 1; i++)
            {
                if (x + i >= _grid.ActualXLength || x + i < 0)
                {
                    continue;
                }
                Tile tile = _tiles[x + i + y * _grid.ActualXLength];
                if (tile.Type == Tile.Room)
                {
                    adjacentToRoom = true;
                }
                if (tile.Type == Tile.Road)
                {
                    adjacentToRoad = true;
                }
            }
            bool horizontal = adjacentToRoad && adjacentToRoom;
            adjacentToRoom = false;
            adjacentToRoad = false;
            for (int j = -1; j <= 1; j++)
            {
                if (y + j >= _grid.ActualYLength || y + j < 0)
                {
                    continue;
                }
                Tile tile = _tiles[x + (y + j) * _grid.ActualXLength];
                if (tile.Type == Tile.Room)
                {
                    adjacentToRoom = true;
                }
                if (tile.Type == Tile.Road)
                {
                    adjacentToRoad = true;
                }
            }
            bool vertical = adjacentToRoad && adjacentToRoom;
            return vertical ^ horizontal;
        }

        public Tile GetTile(int x, int y)
        {
            if (x >= _grid.ActualXLength || x < 0 || y >= _grid.ActualYLength || y < 0)
            {
                return null;
            }
            return _tiles[x + y * _grid.ActualXLength];
        }

        public Tile[] GetTiles()
        {
            return _tiles;
        }

        public List<Room> GetRooms()
        {
            return _rooms;
        }
    }
}