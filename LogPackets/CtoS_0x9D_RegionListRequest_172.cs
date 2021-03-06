using System;
using System.IO;
using System.Text;

namespace PacketLogConverter.LogPackets
{
	[LogPacket(0x9D, 172, ePacketDirection.ClientToServer, "Region list request v172")]
	public class CtoS_0x9D_RegionListRequest_172 : CtoS_0x9D_RegionListRequest
	{
		protected byte slot;
		protected ushort resolution;
		protected ushort options;
		protected byte memory;
		protected ushort unk1;
		protected byte unk2; // unused
		protected uint figureVersion;
		protected byte figureVersion1;
		protected byte skin;
		protected byte genderRace;
		protected byte gender;
		protected byte race;
		protected byte regionExpantions;
		protected byte zero;

		public enum eRegionExpantions: byte
		{
			FoundationsHousing = 0x01,
			NewFrontiers = 0x02,
			ShroudedIsles = 0x08,
			TrialOfAtlantis = 0x10,
			Catacombs = 0x20,
		}

		#region public access properties

		public byte Slot { get { return slot; } }
		public byte Race { get { return race; } }
		public byte Gender { get { return gender; } }

		#endregion

		public override void GetPacketDataString(TextWriter text, bool flagsDescription)
		{
			text.Write("dBslot:{0,-2} flagOption:{1}", slot, flag);
			if (flag > 0)
			{
				int optionsBIT = options;
				optionsBIT = optionsBIT & (0xFFFF ^ 0x0020); // Font name
				optionsBIT = optionsBIT & (0xFFFF ^ 0x0040); // Font size
				optionsBIT = optionsBIT & (0xFFFF ^ 0x0100); // Atlantis TreeFlag
				optionsBIT = optionsBIT & (0xFFFF ^ 0x0200); // OldTerrainFlag
				optionsBIT = optionsBIT & (0xFFFF ^ 0x0400); // Water Options
				optionsBIT = optionsBIT & (0xFFFF ^ 0x0800); // WindowMode
				optionsBIT = optionsBIT & (0xFFFF ^ 0x1000); // SecondDaocCopy
				optionsBIT = optionsBIT & (0xFFFF ^ 0x2000); // Water Options
				optionsBIT = optionsBIT & (0xFFFF ^ 0x4000); // Water Options
				optionsBIT = optionsBIT & (0xFFFF ^ 0x8000); // Dynamic Shadow
				text.Write(" resolutions:0x{0:X4} options:0x{1:X4}(0x{10:X4}) figureVersion:0x{2:X8}{3:X2} memory:{4,2}({9,-2}) unk1:0x{5:X4} skin:0x{6:X2} genderRace:0x{7:X2}(race:{11, -2} gender:{12}) regionExpantions:0x{8:X2}",
					resolution, options, figureVersion, figureVersion1, memory, unk1, skin, genderRace, regionExpantions, memory << 6, optionsBIT, race, gender);
				if (flagsDescription)
				{
					text.Write("\n\tExpantions:");
					byte uRegionregionExpantions = regionExpantions;
					if (regionExpantions > 0)
					{
						byte i = 0;
						foreach(eRegionExpantions eReg in Enum.GetValues(typeof(eRegionExpantions)))
						{
							if ((regionExpantions & (byte)eReg) == (byte)eReg)
							{
								uRegionregionExpantions ^= (byte)eReg;
								if (i++ == 0)
									text.Write(" ");
								else
									text.Write(", ");
								text.Write(eReg.ToString());
							}
						}
					}
					if (uRegionregionExpantions > 0)
						text.Write("\n\tUnknown (regionExpantions:0x{0:X2})", uRegionregionExpantions);

//					str.Append(", Shrouded Isles");
//					str.Append(", Trials of Atlantis");
//					str.Append(", Catacombs");
					string description = string.Format("\n\t{0}*{1}", (resolution >> 8) * 10, (resolution & 0xFF) * 10);
					if ((options & 0x800) == 0x800)
						description += " WindowMode";
					else
						description += " FullScreen";
					if ((options & 0x100) == 0x100)
						description += ", Use Atlantis Trees"; //TreeFlag
					if ((options & 0x200) != 0x200)
						description += ", Use Atlantis Terrain"; // OldTerrainFlag
					if ((options & 0x8000) != 0x8000)
						description += ", Dynamic Shadow"; // OldShadowFlag or Shadow ?
					if ((options & 0x6400) == 0x4400)
						description += ", Classic Water";  // OldRiverFlag
					else if ((options & 0x6400) == 0x2000)
						description += ", Shrouded Isles Water"; // Water1
					else if ((options & 0x6400) == 0x4000) // Water2
						description += ", Reflective Water";
					if ((options & 0x20) == 0x20)
						description += ", Use Classic Name Font";
					if ((options & 0x40) == 0x40)
						description += ", Font Size: Normal";
					else
						description += ", Font Size: Small";
					if (skin == 0)
						description += ", Skin: Shrouded Isles";
					else if (skin == 3)
						description += ", Skin: Atlantis";
					else if (skin == 7)
						description += ", Skin: Custom";
					else
						description += ", Skin: ?" + skin.ToString();
					if ((options & 0x1000) == 0x1000)
						description += ", RuningSecondDaoc";
					text.Write(description);
				}
			}

		}

		/// <summary>
		/// Initializes the packet. All data parsing must be done here.
		/// </summary>
		public override void Init()
		{
			Position = 0;

			slot = ReadByte();
			flag = ReadByte();
			if (flag > 0)
			{
				resolution = ReadShort();
				options = ReadShort();
				memory = ReadByte();
				unk1 = ReadShort();
				unk2 = ReadByte();
				figureVersion = ReadInt();
				figureVersion1 = ReadByte();
				skin = ReadByte();
				genderRace = ReadByte();
				regionExpantions = ReadByte();
				zero = ReadByte();
				gender = 0;
				race = genderRace;
				if (genderRace > 18)
				{
					race = (byte)(genderRace - 18);
					gender = 1;
				}
			}
		}

		/// <summary>
		/// Constructs new instance with given capacity
		/// </summary>
		/// <param name="capacity"></param>
		public CtoS_0x9D_RegionListRequest_172(int capacity) : base(capacity)
		{
		}
	}
}