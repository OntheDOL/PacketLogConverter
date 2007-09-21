using System.IO;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Base class for all filters, encapsulates common logic.
	/// </summary>
	public abstract class AbstractFilter : ILogFilter
	{
		private bool m_active;

		/// <summary>
		/// Activates the filter.
		/// </summary>
		/// <returns><code>true</code> if filter has changed.</returns>
		public virtual bool ActivateFilter()
		{
			IsFilterActive = !IsFilterActive;
			if (IsFilterActive)
				FilterManager.AddFilter(this);
			else
				FilterManager.RemoveFilter(this);
			
			return true;
		}

		/// <summary>
		/// Determines whether the packet should be ignored.
		/// </summary>
		/// <param name="packet">The packet.</param>
		/// <returns>
		/// 	<c>true</c> if packet should be ignored; otherwise, <c>false</c>.
		/// </returns>
		public abstract bool IsPacketIgnored(Packet packet);

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is active; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsFilterActive
		{
			get { return m_active; }
			set { m_active = value; }
		}

		/// <summary>
		/// Serializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is serialized, <code>false</code> otherwise.</returns>
		public virtual bool Serialize(MemoryStream data)
		{
			data.WriteByte((byte)(IsFilterActive ? 1 : 0));
			return true;
		}

		/// <summary>
		/// Deserializes data of instance of this filter.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns><code>true</code> if filter is deserialized, <code>false</code> otherwise.</returns>
		public virtual bool Deserialize(MemoryStream data)
		{
			int active = data.ReadByte();
			IsFilterActive = 0 != active;
			return true;
		}
	}
}