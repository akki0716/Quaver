using System.Collections.Generic;
using Quaver.API.Enums;
using Quaver.API.Helpers;
using Quaver.Shared.Assets;
using Quaver.Shared.Modifiers;
using Quaver.Shared.Modifiers.Mods;
using Wobble.Graphics;
using Wobble.Graphics.UI.Form;

namespace Quaver.Shared.Screens.Selection.UI.Modifiers.Components
{
    public class SelectableModifierSpeed : SelectableModifier
    {
        /// <summary>
        ///     Horizontal selector to change the rate of the audio
        /// </summary>
        private HorizontalSelector RateChanger { get; }

        /// <summary>
        ///     All of the available speeds to play
        /// </summary>
        private List<string> Speeds { get; } = new List<string>()
        {
            "0.5x",
            "0.55x",
            "0.6x",
            "0.65x",
            "0.7x",
            "0.75x",
            "0.8x",
            "0.85x",
            "0.9x",
            "0.95x",
            "1.0x",
            "1.1x",
            "1.2x",
            "1.3x",
            "1.4x",
            "1.5x",
            "1.6x",
            "1.7x",
            "1.8x",
            "1.9x",
            "2.0x"
        };

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="width"></param>
        public SelectableModifierSpeed(int width) : base(width, new ModSpeed(ModIdentifier.Speed11X))
        {
            RateChanger = new HorizontalSelector(Speeds, new ScalableVector2(100, 32), Fonts.Exo2SemiBold, 16,
                FontAwesome.Get(FontAwesomeIcon.fa_chevron_pointing_to_the_left),
                FontAwesome.Get(FontAwesomeIcon.fa_right_chevron), new ScalableVector2(20, 20), 0, OnSelected, GetSelectedIndex())
            {
                Parent = this,
                Alignment = Alignment.MidRight,
                X = -30,
                UsePreviousSpriteBatchOptions = true,
                Alpha = 0,
                ButtonSelectLeft = {UsePreviousSpriteBatchOptions = true},
                ButtonSelectRight = {UsePreviousSpriteBatchOptions = true},
                SelectedItemText =
                {
                    UsePreviousSpriteBatchOptions = true,
                    Tint = Mod.ModColor
                }
            };

            ModManager.ModsChanged += OnModsChanged;
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public override void Destroy()
        {
            ModManager.ModsChanged -= OnModsChanged;
            base.Destroy();
        }

        /// <summary>
        ///     Gets the selected index of the speeds based on the audio rate
        /// </summary>
        /// <returns></returns>
        private int GetSelectedIndex()
        {
            var rate = ModHelper.GetRateFromMods(ModManager.Mods);
            string rateString;

            switch (rate)
            {
                case 1f:
                    rateString = "1.0x";
                    break;
                case 2:
                    rateString = "2.0x";
                    break;
                default:
                    rateString = $"{rate}x";
                    break;
            }

            return Speeds.FindIndex(x => x == rateString);
        }

        /// <summary>
        ///     Called when the selected value changes
        /// </summary>
        /// <param name="val"></param>
        /// <param name="index"></param>
        private static void OnSelected(string val, int index)
        {
            if (val == "1.0x")
            {
                ModManager.RemoveSpeedMods();
                return;
            }

            ModManager.AddMod(ModHelper.GetModsFromRate(float.Parse(val.Replace("x", ""))));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnModsChanged(object sender, ModsChangedEventArgs e)
        {
            RateChanger.SelectedIndex = GetSelectedIndex();
            RateChanger.SelectedItemText.Text = Speeds[GetSelectedIndex()];
        }
    }
}