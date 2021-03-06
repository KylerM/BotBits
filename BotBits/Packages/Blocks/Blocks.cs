﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BotBits.Events;
using BotBits.SendMessages;

namespace BotBits
{
    public sealed class Blocks : EventListenerPackage<Blocks>, IBlockAreaEnumerable,
        IWorld<BlockData<ForegroundBlock>, BlockData<BackgroundBlock>>
    {
        private BlockDataWorld _blockDataWorld;
        private ReadOnlyBlocksWorld _readOnlyBlocksWorld;

        [Obsolete("Invalid to use \"new\" on this class. Use the static .Of(BotBits) method instead.", true)]
        public Blocks()
        {
            this.World = new BlockDataWorld(0, 0);
        }

        private BlockDataWorld World
        {
            get { return this._blockDataWorld; }
            set
            {
                this._blockDataWorld = value;
                this._readOnlyBlocksWorld = new ReadOnlyBlocksWorld(this._blockDataWorld);
            }
        }

        public IEnumerator<BlocksItem> GetEnumerator()
        {
            return this.In(this.Area).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        Blocks IBlockAreaEnumerable.Blocks
        {
            get { return this; }
        }

        public Rectangle Area
        {
            get { return new Rectangle(0, 0, this.Width, this.Height); }
        }

        public int Height
        {
            get { return this._blockDataWorld.Height; }
        }

        public int Width
        {
            get { return this._blockDataWorld.Width; }
        }

        public IBlockLayer<BlockData<ForegroundBlock>> Foreground
        {
            get { return this._readOnlyBlocksWorld.Foreground; }
        }

        public IBlockLayer<BlockData<BackgroundBlock>> Background
        {
            get { return this._readOnlyBlocksWorld.Background; }
        }

        public BlocksItem At(Point point)
        {
            return this.At(point.X, point.Y);
        }

        public BlocksItem At(int x, int y)
        {
            return new BlocksItem(this, x, y);
        }

        public void Place(int x, int y, BackgroundBlock block)
        {
            new PlaceSendMessage(Layer.Background, x, y, (int) block.Id)
                .SendIn(this.BotBits);
        }

        public void Place(int x, int y, ForegroundBlock block)
        {
            new PlaceSendMessage(Layer.Foreground, x, y, (int) block.Id, block.GetArgs())
                .SendIn(this.BotBits);
        }

        [EventListener]
        private void On(InitEvent e)
        {
            this.World = BlockUtils.GetWorld(e.PlayerIOMessage, e.WorldWidth, e.WorldHeight);
        }

        [EventListener(EventPriority.Low)]
        private void OnLow(InitEvent e)
        {
            new WorldResizeEvent(e.WorldWidth, e.WorldHeight)
                .RaiseIn(this.BotBits);
        }

        [EventListener]
        private void On(LoadLevelEvent e)
        {
            this.World = BlockUtils.GetWorld(e.PlayerIOMessage, this.Width, this.Height);
        }

        [EventListener]
        private void On(ClearEvent e)
        {
            this.World = BlockUtils.GetClearedWorld(e.RoomWidth, e.RoomHeight, e.BorderBlock);
            new WorldResizeEvent(e.RoomWidth, e.RoomHeight)
                .RaiseIn(this.BotBits);
        }

        [EventListener]
        private void On(BlockPlaceEvent e)
        {
            if (this.Height <= e.Y || this.Width <= e.X) return;

            switch (e.Layer)
            {
                case Layer.Foreground:
                    this.RaiseBlock(e.X, e.Y, (Foreground.Id) e.Id, e.Player);
                    break;

                case Layer.Background:
                    this.RaiseBackground(e.X, e.Y, new BackgroundBlock((Background.Id) e.Id), e.Player);
                    break;
            }
        }

        [EventListener]
        private void On(PortalPlaceEvent e)
        {
            this.RaisePortalBlock(e.X, e.Y, (Foreground.Id) e.Id, e.PortalId, e.PortalTarget, e.PortalRotation, e.Player);
        }

        [EventListener]
        private void On(CoinDoorPlaceEvent e)
        {
            this.RaiseNumberBlock(e.X, e.Y, (Foreground.Id) e.Id, e.CoinsToOpen, e.Player);
        }

        [EventListener]
        private void On(MorphablePlaceEvent e)
        {
            this.RaiseNumberBlock(e.X, e.Y, (Foreground.Id) e.Id, e.Rotation, e.Player);
        }

        [EventListener]
        private void On(SoundPlaceEvent e)
        {
            this.RaiseSignedNumberBlock(e.X, e.Y, (Foreground.Id)e.Id, e.SoundId, e.Player);
        }

        [EventListener]
        private void On(WorldPortalPlaceEvent e)
        {
            this.RaiseStringBlock(e.X, e.Y, (Foreground.Id) e.Id, e.WorldPortalTarget, e.Player);
        }

        [EventListener]
        private void On(LabelPlaceEvent e)
        {
            this.RaiseLabelBlock(e.X, e.Y, (Foreground.Id) e.Id, e.Text, e.TextColor, e.Player);
        }

        [EventListener]
        private void On(SignPlaceEvent e)
        {
            this.RaiseSignBlock(e.X, e.Y, (Foreground.Id) e.Id, e.Text, e.SignColor, e.Player);
        }

        private void RaiseBlock(int x, int y, Foreground.Id block, Player player)
        {
            this.RaiseForeground(x, y, new ForegroundBlock(block), player);
        }

        private void RaiseNumberBlock(int x, int y, Foreground.Id block, uint args, Player player)
        {
            this.RaiseForeground(x, y, new ForegroundBlock(block, args), player);
        }

        private void RaiseSignedNumberBlock(int x, int y, Foreground.Id block, int args, Player player)
        {
            this.RaiseForeground(x, y, new ForegroundBlock(block, args), player);
        }

        private void RaiseStringBlock(int x, int y, Foreground.Id block, string text, Player player)
        {
            this.RaiseForeground(x, y, new ForegroundBlock(block, text), player);
        }

        private void RaiseLabelBlock(int x, int y, Foreground.Id block, string text, string textColor, Player player)
        {
            this.RaiseForeground(x, y, new ForegroundBlock(block, text, textColor), player);
        }
        
        private void RaiseSignBlock(int x, int y, Foreground.Id block, string text, Morph.Id signColor, Player player)
        {
            this.RaiseForeground(x, y, new ForegroundBlock(block, text, signColor), player);
        }
        
        private void RaisePortalBlock(int x, int y, Foreground.Id block,
            uint portalId, uint portalTarget, Morph.Id portalRotation, Player player)
        {
            this.RaiseForeground(x, y, new ForegroundBlock(block, portalId, portalTarget, portalRotation), player);
        }

        private void RaiseForeground(int x, int y, ForegroundBlock newBlock, Player player)
        {
            var oldData = this.World.Foreground[x, y];
            var newData = new BlockData<ForegroundBlock>(player, newBlock);
            this.World.Foreground[x, y] = newData;

            new ForegroundPlaceEvent(x, y, oldData, newData)
                .RaiseIn(this.BotBits);
        }

        private void RaiseBackground(int x, int y, BackgroundBlock newBlock, Player player)
        {
            var oldData = this.World.Background[x, y];
            var newData = new BlockData<BackgroundBlock>(player, newBlock);
            this.World.Background[x, y] = newData;

            new BackgroundPlaceEvent(x, y, oldData, newData)
                .RaiseIn(this.BotBits);
        }

        public IWorld GetWorld()
        {
            return new ProxyWorld(this);
        }

        private class ProxyWorld : IWorld
        {
            private readonly Blocks _innerWorld;

            public ProxyWorld(Blocks innerWorld)
            {
                this._innerWorld = innerWorld;
            }

            public int Width
            {
                get { return this._innerWorld.Width; }
            }

            public int Height
            {
                get { return this._innerWorld.Height; }
            }

            public IBlockLayer<ForegroundBlock> Foreground
            {
                get { return new ProxyLayer<ForegroundBlock>(this._innerWorld.Foreground); }
            }

            public IBlockLayer<BackgroundBlock> Background
            {
                get { return new ProxyLayer<BackgroundBlock>(this._innerWorld.Background); }
            }
        }

        private class ProxyLayer<T> : IBlockLayer<T> where T : struct
        {
            private readonly IBlockLayer<BlockData<T>> _innerLayer;

            public ProxyLayer(IBlockLayer<BlockData<T>> innerLayer)
            {
                this._innerLayer = innerLayer;
            }

            public T this[Point p]
            {
                get { return this._innerLayer[p].Block; }
            }

            public T this[int x, int y]
            {
                get { return this._innerLayer[x, y].Block; }
            }

            public int Height
            {
                get { return this._innerLayer.Height; }
            }

            public int Width
            {
                get { return this._innerLayer.Width; }
            }

            public IEnumerator<LayerItem<T>> GetEnumerator()
            {
                return this._innerLayer
                    .Select(item => new LayerItem<T>(item.Location, item.Data.Block))
                    .GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}