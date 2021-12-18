using System.Runtime.InteropServices;

namespace TUF_Keyboard_Extension
{
    public class KeyboardAPI
    {
        public enum DevControlFunckType
        {
            ASWMI_NB_DEVCONTROL_2ARG = 0x100056,
            ASWMI_NB_DEVCONTROL = 0x100057
        };
        public enum EffectType
        {
            STATIC = 0x00,
            BREATHING = 0x01,
            COLOR_CYCLE = 0x02,
            STROBING = 0x0a
        };
        public enum Tempo
        {
            NONE = 0x00,
            FAST = 0xf5,
            MEDIUM = 0xeb,
            SLOW = 0xe1
        };
        public enum BacklightPowerSettings
        {
            BOOT_UP = 0x3,
            AWAKE = 0xc,
            SLEEP = 0x30,
            SHUT_DOWN = 0xc0,
        };

        public Tempo СorrectTempo(EffectType effectType, Tempo tempo)
        {
            if (effectType == EffectType.STATIC || effectType == EffectType.STROBING)
            {
                tempo = Tempo.NONE;
            }

            if ((effectType == EffectType.COLOR_CYCLE || effectType == EffectType.BREATHING) && tempo == Tempo.NONE)
            {
                tempo = Tempo.SLOW;
            }
            return tempo;
        }

        public void ApplyConfig(DevControlFunckType devControlFunckType, uint RGDiodeAndEType, uint BDiodeAndTemp, uint backlightPowerSettings)
        {
            switch (devControlFunckType)
            {
                case DevControlFunckType.ASWMI_NB_DEVCONTROL:
                    AsWMI_DeviceControlBacklightPowerSettings((uint)DevControlFunckType.ASWMI_NB_DEVCONTROL, backlightPowerSettings);
                    break;

                case DevControlFunckType.ASWMI_NB_DEVCONTROL_2ARG:
                    AsWMI_DeviceControlBacklightEffectTypeTempoColor((uint)DevControlFunckType.ASWMI_NB_DEVCONTROL_2ARG, RGDiodeAndEType, BDiodeAndTemp);
                    break;
            }
        }

        public void SetLaptopRGBBacklight(EffectType effectType, Tempo tempo, uint brightnessRedDiode, uint brightnessGreenDiode, uint brightnessBueDiode)
        {
            uint correctedTempo = (uint)СorrectTempo(effectType, tempo);
            uint RGDiodeAndEType = (0x00010000 * brightnessRedDiode) + (0x01000000 * brightnessGreenDiode) + (0x00000100 * (uint)effectType) + 0xb3;
            uint BDiodeAndTemp = brightnessBueDiode + (0x0100 * correctedTempo);

            ApplyConfig(DevControlFunckType.ASWMI_NB_DEVCONTROL_2ARG, RGDiodeAndEType, BDiodeAndTemp, 0);
        }

        public void Start()
        {
            AsWMI_Open();
        }

        public void Stop()
        {
            try
            {
                AsWMI_Close();
            }
            catch (System.Exception)
            {
                System.Console.WriteLine("Failed to close;");
            }
        }


        [DllImport(@"ACPIWMI.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "AsWMI_NB_DeviceControl_2arg")]
        private extern static int AsWMI_DeviceControlBacklightEffectTypeTempoColor(uint one, uint two, uint three);

        [DllImport(@"ACPIWMI.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "AsWMI_NB_DeviceControl")]
        private extern static int AsWMI_DeviceControlBacklightPowerSettings(uint one, uint two);

        [DllImport(@"ACPIWMI.dll", CallingConvention = CallingConvention.StdCall)]
        private extern static int AsWMI_Open();

        [DllImport(@"ACPIWMI.dll", CallingConvention = CallingConvention.StdCall)]
        private extern static int AsWMI_Close();
    }
}
