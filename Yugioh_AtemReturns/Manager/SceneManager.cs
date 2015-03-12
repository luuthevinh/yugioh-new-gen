﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yugioh_AtemReturns.GameObjects;

namespace Yugioh_AtemReturns.Manager
{
    public class SceneManager
    {
        private List<Scene> m_ListScene = new List<Scene>();
        private static SceneManager m_Instance;
        private ContentManager m_Content;

        public ContentManager Content {
            get { return m_Content; }
            set { m_Content = value; }
        }
        private SceneManager()
        {

        }

        public static SceneManager GetInstance()
        {
            if(m_Instance == null)
            {
                m_Instance = new SceneManager();
            }
            return m_Instance;
        }

        public void Init()
        { 
            if(m_ListScene.Count() != 0)
            {
                m_ListScene.Last().Init(this.Content);
            }
        }
        public void Update(GameTime gametime)
        {
            if (m_ListScene.Count() != 0)
            {
                m_ListScene.Last().Update(gametime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (m_ListScene.Count() != 0)
            {
                m_ListScene.Last().Draw(spriteBatch);
            }
        }

        public void AddScene(Scene scene)
        {
            m_ListScene.Add(scene);
            m_ListScene.Last().Init(m_Content);
        }

        public void RemoveScene()
        {
            if(m_ListScene.Count() != 0)
            {
                m_ListScene.RemoveAt(m_ListScene.Count() - 1);
            }
        }
        public void ReplaceScene(Scene scene)
        {
            if(m_ListScene.Count == 0)
            {
                AddScene(scene);
            }
            else
            {
                RemoveScene();
                AddScene(scene);
            }
        }
    }
}