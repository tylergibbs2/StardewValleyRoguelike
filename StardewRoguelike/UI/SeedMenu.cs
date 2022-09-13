using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Locations;
using StardewValley.Menus;
using System;

namespace StardewRoguelike.UI
{
    internal class SeedInputBox : TextBox
    {
        public SeedInputBox(SpriteFont font, Color textColor) : base(null, null, font, textColor) { }

        public override void RecieveTextInput(char inputChar)
        {
            if (Text.Length == 0 && inputChar == '-')
            {
                Text += inputChar;
                return;
            }

            if (!Selected || !char.IsDigit(inputChar))
                return;

            Text += inputChar;
        }
    }

    internal class SeedMenu : IClickableMenu
    {
        private SeedInputBox textBox;

        private ClickableComponent textBoxComponent;

        private ClickableTextureComponent randomButton;

        private ClickableTextureComponent doneButton;

        public SeedMenu()
        {
            textBox = new(Game1.smallFont, Game1.textColor);
            textBox.OnEnterPressed += textBoxEnter;
            Game1.keyboardDispatcher.Subscriber = textBox;
            textBox.Selected = true;

            textBoxComponent = new(Rectangle.Empty, "")
            {
                myID = 101,
                rightNeighborID = 102
            };
            randomButton = new(Rectangle.Empty, Game1.mouseCursors, new Rectangle(381, 361, 10, 10), 4f)
            {
                myID = 102,
                leftNeighborID = 101,
                rightNeighborID = 103
            };
            doneButton = new(Rectangle.Empty, Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46), 1f)
            {
                myID = 103,
                leftNeighborID = 102
            };

            CalculatePositions();
            textBox.Text = Roguelike.FloorRngSeed.ToString();

            populateClickableComponentList();

            if (Game1.options.gamepadControls)
                snapToDefaultClickableComponent();
        }

        private void CalculatePositions()
        {
            width = 600;
            height = 150;

            xPositionOnScreen = Game1.uiViewport.Width / 2 - (width / 2);
            yPositionOnScreen = Game1.uiViewport.Height / 2 - (height / 2) - 100;

            textBox.X = xPositionOnScreen + 32;
            textBox.Y = yPositionOnScreen + 54;
            textBox.Width = 370;
            textBox.Height = 186;

            textBoxComponent.bounds = new(textBox.X, textBox.Y, 400, 75);

            randomButton.bounds = new(xPositionOnScreen + width - 150, yPositionOnScreen + 54, 64, 64);
            doneButton.bounds = new(xPositionOnScreen + width - 90, yPositionOnScreen + 42, 64, 64);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            base.gameWindowSizeChanged(oldBounds, newBounds);
            CalculatePositions();
        }

        public override void snapToDefaultClickableComponent()
        {
            currentlySnappedComponent = textBoxComponent;
            snapCursorToCurrentSnappedComponent();
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            doneButton.tryHover(x, y);
            randomButton.tryHover(x, y);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);
            textBox.Update();
            if (doneButton.containsPoint(x, y))
            {
                textBoxEnter(textBox);
                Game1.playSound("smallSelect");
            }
            else if (randomButton.containsPoint(x, y))
            {
                textBox.Text = Guid.NewGuid().GetHashCode().ToString();
                Game1.playSound("drumkit6");
            }
        }

        private void textBoxEnter(TextBox sender)
        {
            if (sender.Text.Length >= 1)
            {
                Roguelike.FloorRngSeed = int.Parse(sender.Text);
                Roguelike.FloorRng = new(Roguelike.FloorRngSeed);
                ChallengeFloor.History.Clear();
                Roguelike.SeenMineMaps.Clear();
                MineShaft.clearActiveMines();

                Game1.exitActiveMenu();
            }
        }

        public override void draw(SpriteBatch b)
        {
            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);

            SpriteText.drawStringWithScrollCenteredAt(b, "Seed", Game1.uiViewport.Width / 2, yPositionOnScreen - 70, "Seed");

            drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                xPositionOnScreen,
                yPositionOnScreen,
                width,
                height,
                Color.White,
                drawShadow: true
            );

            textBox.Draw(b);
            doneButton.draw(b);
            randomButton.draw(b);

            drawMouse(b);
        }
    }
}
