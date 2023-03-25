using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroStore.Models.Enums {
    public enum Platform {
        NES,

        SNES,

        [Description("Genesis")]
        GENESIS,

        [Description("GameBoy")]
        GAMEBOY,

        [Description("GameBoy Color")]
        GAMEBOY_COLOR,

        [Description("Game Gear")]
        GAME_GEAR
    }
}
