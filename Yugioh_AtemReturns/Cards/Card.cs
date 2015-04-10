﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Yugioh_AtemReturns.GameObjects;
using Yugioh_AtemReturns.Manager;
using Microsoft.Xna.Framework;
using Yugioh_AtemReturns.Cards.Monsters;
using Yugioh_AtemReturns.Cards.Traps;
using Yugioh_AtemReturns.Cards.Spells;
using Yugioh_AtemReturns.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace Yugioh_AtemReturns.Cards
{
    delegate void CardLeftClickEventHandle(Card sender, EventArgs e);
    delegate void CardHoveredEventHandle(Card sender, EventArgs e);
    abstract class Card : MyObject
    {

        private eCardType cardType;
        private bool isFaceUp;
        private Button button;
        
        public event CardLeftClickEventHandle LeftClick;
        public event CardLeftClickEventHandle RightClick;
        public event CardHoveredEventHandle Hovered;
        public event CardHoveredEventHandle OutHovered;

        public virtual void OnLeftClick(EventArgs e)
        {
            if (LeftClick != null)
                LeftClick(this, e);
        }
        public virtual void OnRightClick(EventArgs e)
        {
            if (RightClick != null)
                RightClick(this, e);
        }
        public virtual void OnHovered(EventArgs e)
        {
            if (Hovered != null)
                Hovered(this, e);
        }
        public virtual void outHovered(EventArgs e)
        {
            if (OutHovered != null)
                OutHovered(this, e);
        }
        #region Property
        public bool IsFaceUp
        {
            get { return isFaceUp; }
            set 
            {
                if (value == true)
                {
                    this.Sprite.Texture = this.m_frontsideTexture;
                }
                else
                {
                    this.Sprite.Texture = this.m_backsideTexture;
                }
                isFaceUp = value; 
            }
        }
        public eCardType CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        public Button Button
        {
            get { return button; }
            set { button = value; }
        }
        public STATUS STATUS
        {
            get { return base.Status; }
            set
            {
                base.Status = value;
                if (value == STATUS.DEF)
                    this.IsFaceUp = false;
                if (value == STATUS.ATK)
                    this.IsFaceUp = true;
            }
        }
        #endregion
        private Texture2D m_backsideTexture;
        private Texture2D m_frontsideTexture;
        public Card(ContentManager _content, ID _id, SpriteID _spriteId, eCardType _cardType)
            : base(_content, _id, _spriteId)
        {
            m_backsideTexture = SpriteManager.getInstance(_content).GetTexture(SpriteID.CBackSide);
            m_frontsideTexture = SpriteManager.getInstance(_content).GetTexture(_spriteId); 
            this.IsFaceUp = true;
            this.CardType = _cardType;
            this.Button = new Button(this.Sprite);
            this.Button.Position = this.Position;
            this.Button.ButtonEvent += new Action(Button_DoActionClick);
            this.Button.RightClick += new Action(Button_OnRightClick);
            this.Hovered += new CardHoveredEventHandle(Card_Hovered); 
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch _spritebatch)
        {
            base.Draw(_spritebatch);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.Button.Update(gameTime);

            if (this.Button.Hovered)
            {
                this.OnHovered(EventArgs.Empty);
            }
            else
            {
                this.outHovered(EventArgs.Empty);
            }
        }

        public override string ToString()
        {
            switch (this.CardType)
            {
                case eCardType.MONSTER:
                    return (this as Monster).Original.Id.ToString();
                case eCardType.TRAP:
                    return (this as Trap).Original.Id.ToString();
                case eCardType.SPELL:
                    return (this as Spell).Original.Id.ToString();
                case eCardType.XYZ:
                    throw new Exception("Chua dinh nghia Xyz monster");
                case eCardType.SYNCHRO:
                    throw new Exception("Chua dinh nghia Synchro monster");
                case eCardType.FUSION:
                    throw new Exception("Chua dinh nghia Fusion monster");
                default:
                    return String.Empty ;
            }
        }
        private void Button_DoActionClick()
        {
            this.OnLeftClick(EventArgs.Empty);
        }
        private void Button_OnRightClick()
        {
            this.OnRightClick(EventArgs.Empty);
        }
        private void Card_Hovered(Card sender, EventArgs e)
        {
            if (sender.IsFaceUp)
                PlayScene.DetailSideBar.SetCardPreview(sender.ToString());
            else
                PlayScene.DetailSideBar.SetDefaultCardPreview();
        }
    }
}
