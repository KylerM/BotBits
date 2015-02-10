﻿using System;
using System.Collections.Generic;
using PlayerIOClient;

namespace BotBits
{
    public static class WorldUtils
    {
        private const uint InitOffset = 19;

        internal static World GetWorld(Message m, int width, int height, uint offset = InitOffset)
        {
            var world = new World(width, height);
            uint pointer = GetStart(m, offset);

            string strValue2;
            while ((strValue2 = m[pointer] as string) == null || strValue2 != "we")
            {
                var block = m.GetInteger(pointer++);
                var l = (Layer)m.GetInteger(pointer++);
                byte[] byteArrayX = m.GetByteArray(pointer++);
                byte[] byteArrayY = m.GetByteArray(pointer++);

                switch (l)
                {
                    case Layer.Background:
                        var bgWorldBlock = new BackgroundBlock((Background)block);
                        foreach (Point pos in GetPos(byteArrayX, byteArrayY))
                            world.Background[pos.X, pos.Y] = bgWorldBlock;
                        break;

                    case Layer.Foreground:
                        ForegroundBlock foregroundBlock;
                        BlockArgsType blockArgsType = GetBlockArgsType(GetForegroundType((Foreground)block));

                        switch (blockArgsType)
                        {
                            case BlockArgsType.None:
                                foregroundBlock = new ForegroundBlock((Foreground)block);
                                break;

                            case BlockArgsType.Number:
                                uint i = m.GetUInt(pointer++);
                                foregroundBlock = new ForegroundBlock((Foreground)block, i);
                                break;

                            case BlockArgsType.String:
                                string str = m.GetString(pointer++);
                                foregroundBlock = new ForegroundBlock((Foreground)block, str);
                                break;

                            case BlockArgsType.Portal:
                                var portalRotation = (PortalRotation)m.GetUInt(pointer++);
                                uint portalId = m.GetUInt(pointer++);
                                uint portalTarget = m.GetUInt(pointer++);
                                foregroundBlock = new ForegroundBlock((Foreground)block, portalId, portalTarget, portalRotation);
                                break;

                            default:
                                throw new NotSupportedException("Invalid block.");
                        }

                        foreach (Point pos in GetPos(byteArrayX, byteArrayY))
                            world.Foreground[pos.X, pos.Y] = foregroundBlock;
                        break;
                }
            }

            return world;
        }

        private static uint GetStart(Message m, uint offset)
        {
            uint start = 0;
            for (uint i = offset; i <= m.Count - 1; i++)
            {
                string strValue;
                if ((strValue = m[i] as string) != null && strValue == "ws")
                {
                    start = i + 1;
                    break;
                }
            }
            return start;
        }

        private static IEnumerable<Point> GetPos(byte[] byteArrayX, byte[] byteArrayY)
        {
            for (int i = 0; i <= byteArrayX.Length - 1; i += 2)
            {
                int x = byteArrayX[i] * 256 + byteArrayX[i + 1];
                int y = byteArrayY[i] * 256 + byteArrayY[i + 1];

                yield return new Point(x, y);
            }
        }

        public static World GetClearedWorld(int width, int height, Foreground borderBlock)
        {
            var world = new World(width, height);

            // Border drawing
            int maxX = width - 1;
            int maxY = height - 1;
            var block = new ForegroundBlock(borderBlock);
            for (int y = 0; y <= maxY; y++)
            {
                world.Foreground[0, y] = block;
                world.Foreground[maxX, y] = block;
            }
            for (int x = 0; x <= maxX; x++)
            {
                world.Foreground[x, 0] = block;
                world.Foreground[x, maxY] = block;
            }

            return world;
        }

        public static ForegroundType GetForegroundType(Foreground id)
        {
            switch (id)
            {
                case Foregrounds.Door.Coin:
                case Foregrounds.Gate.Coin:
                case Foregrounds.Door.BlueCoin:
                case Foregrounds.Gate.BlueCoin:
                    return ForegroundType.CoinDoor;

                case Foregrounds.Hazard.Spike:
                    return ForegroundType.Spike;

                case Foregrounds.SciFi2013.BlueStraight:
                case Foregrounds.SciFi2013.YellowStraight:
                case Foregrounds.SciFi2013.GreenStraight:
                    return ForegroundType.SciFiStraight;

                case Foregrounds.SciFi2013.YellowSlope:
                case Foregrounds.SciFi2013.GreenSlope:
                case Foregrounds.SciFi2013.BlueSlope:
                    return ForegroundType.SciFiSlope;

                case Foregrounds.Music.Piano:
                    return ForegroundType.Piano;

                case Foregrounds.Music.Drum:
                    return ForegroundType.Drum;

                case Foregrounds.Portal.Invisible:
                case Foregrounds.Portal.Normal:
                    return ForegroundType.Portal;

                case Foregrounds.Portal.World:
                    return ForegroundType.WorldPortal;

                case Foregrounds.Admin.Text:
                    return ForegroundType.Text;
                    
                case Foregrounds.Sign.Block:
                    return ForegroundType.Sign;

                default:
                    return ForegroundType.Normal;
            }
        }

        public static BlockArgsType GetBlockArgsType(ForegroundType type)
        {
            switch (type)
            {
                case ForegroundType.Normal:
                    return BlockArgsType.None;

                case ForegroundType.CoinDoor:
                case ForegroundType.Drum:
                case ForegroundType.Piano:
                case ForegroundType.Spike:
                case ForegroundType.SciFiSlope:
                case ForegroundType.SciFiStraight:
                    return BlockArgsType.Number;

                case ForegroundType.Sign:
                case ForegroundType.Text:
                case ForegroundType.WorldPortal:
                    return BlockArgsType.String;

                case ForegroundType.Portal:
                    return BlockArgsType.Portal;

                default:
                    throw new ArgumentException("Invalid BlockType.", "type");
            }
        }
    }
}