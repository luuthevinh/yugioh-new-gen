﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Yugioh_AtemReturns.Manager;

namespace Yugioh_AtemReturns.GameObjects
{
    /// <summary>
    /// Button có các chức năng Selected, Hovered, thực hiện sự kiện.
    /// Muốn tạo sự kiện cho Button click vào thì dùng ButtonEvent. Ví dụ:
    /// testButton.ButtonEvent += new Action(doSomething); 
    /// * hàm doSomething là hàm kiểu void() *
    /// </summary>
    public class Button : MyObject //BaseObject
    {
        private Sprite normalImage;
        private Sprite selectedImage;
        private Sprite hoverImage;

        private bool isHovered = false;
        private bool isSelected = false;

        #region Properties
        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
                if(this.Sprite != null)
                    this.Sprite.Position = this.Position;
                if(this.normalImage != null)
                    normalImage.Position = this.Position;
                if (this.hoverImage != null)
                    hoverImage.Position = this.Position;
                if (this.selectedImage != null)
                    selectedImage.Position = this.Position;
            }
        }
        public bool Selected 
        {
            get { return isSelected; }
            set { 
                isSelected = value;
                this.Sprite = selectedImage;
            }
        }
        public bool Hovered {
            get { return isHovered; }
            set 
            { 
                isHovered = value;
                this.Sprite = hoverImage;
            }
        }
        #endregion

        public Button(Sprite image)
        {
            this.Sprite = image;
        }
        public Button(Sprite normal, Sprite hover)
        {
            normalImage = normal;
            hoverImage = hover;

            this.Sprite = normalImage;
        }
        public Button(Sprite normal, Sprite hover, Sprite selected)
        {
            normalImage = normal;
            hoverImage = hover;
            selectedImage = selected;

            this.Sprite = normalImage;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //////////////////
            this.Sprite.Draw(spriteBatch);
        }
        
        public override void Update(GameTime gameTime)
        {
            //Mouse
            this.checkMouseUpdate();
        }

        private void checkMouseUpdate()
        {
            InputController.getInstance().Begin(); //Begin get input

            if (InputController.getInstance().IsLeftClick() && isHovered )
            {
                Debug.WriteLine(String.Format("{0},{1}", InputController.getInstance().MousePosition.X, InputController.getInstance().MousePosition.Y));
                this.isSelected = true;
                this.doButtonEvent();
            }
            else if (InputController.getInstance().IsLeftClick() && !isHovered)
            {
                this.isSelected = false;
            }

            if (this.Sprite.Bound.Contains(InputController.getInstance().MousePosition) && !isHovered)
            {
                Debug.WriteLine("hovered!");
                this.Sprite = hoverImage;
                isHovered = true;
            }

            if (!this.Sprite.Bound.Contains(InputController.getInstance().MousePosition))
            {
                isHovered = false;
                this.Sprite = normalImage;
            }
            
            if(isSelected)
            {
                this.Sprite = selectedImage;
            }

            InputController.getInstance().End(); //End get input
        }

        public event Action ButtonEvent;

        public void doButtonEvent()
        {
            if (ButtonEvent != null)
                ButtonEvent();
        }
    }
}