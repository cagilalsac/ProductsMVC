namespace CORE.APP.Domain
{
    /// <summary>
    /// Abstract base class for all entities.
    /// </summary>
    public abstract class Entity
    {
        // Way 1:
        // Field to store the entity's ID.
        //private int id; // variable, field

        // Method to get the entity's ID.
        //public int GetId() // function, method, behavior
        //{
        //    return id;
        //}

        // Method to set the entity's ID.
        //public void SetId(int id)
        //{
        //    this.id = id;
        //}



        // Way 2:
        /// <summary>
        /// Property that gets or sets the ID of the entity to be used as the primary key in the database table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Property that gets or sets the GUID of the entity to be used for a unique string identifier such as: "0f8fad5b-d9cb-469f-a165-70867728950e".
        /// May be used for getting the entity instead of Id, file names, etc.
        /// </summary>
        public string Guid { get; set; }
    }
}
