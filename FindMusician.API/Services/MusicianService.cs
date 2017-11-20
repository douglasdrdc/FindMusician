using FindMusician.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindMusician.API.Services
{
    public class MusicianService
    {
        private List<Musician> Musicians { get; set; }

        public MusicianService()
        {
            this.Musicians = new List<Musician>();
            this.Musicians.Add(new Musician() { Id = 1, FirstName = "Douglas", LastName = "Campos", DataNascimento = new DateTime(1987, 08, 18) });
            this.Musicians.Add(new Musician() { Id = 2, FirstName = "Edson", LastName = "Oliveira", DataNascimento = new DateTime(1986, 11, 21) });
        }

        public List<Musician> Get()
        {   
            return this.Musicians;
        }
        
        public Musician Get(int id)
        {
            return this.Musicians.Where(x => x.Id == id).FirstOrDefault();
        }

        public void Add(Musician musician)
        {
            if (musician.Id != 0)
                throw new ApplicationException("O Id informado é inválido.");

            if (DateTime.Now.AddYears(-18) < musician.DataNascimento)
                throw new ApplicationException("Para se cadastrar é necessário ter mais de 18 anos.");

            var maxId = this.Musicians.Select(x => x.Id).Max();
            musician.Id = maxId + 1;
            this.Musicians.Add(musician);
        }

        public void Update(Musician musician)
        {
            if (musician.Id == 0 || this.Musicians.Where(x => x.Id == musician.Id).Count() != 1)
                throw new ApplicationException("O registro informado não existe na base de dados.");
            
            if (DateTime.Now.AddYears(-18) < musician.DataNascimento)
                throw new ApplicationException("Para se cadastrar é necessário ter mais de 18 anos.");

            var musicianSelect = this.Musicians.Where(x => x.Id == musician.Id).FirstOrDefault();
            this.Musicians.Remove(musicianSelect);

            this.Musicians.Add(musician);
        }

        public void Delete(int id)
        {
            if (id == 0 || this.Musicians.Where(x => x.Id == id).Count() != 1)
                throw new ApplicationException("O registro informado não existe na base de dados.");
                        
            var musicianSelect = this.Musicians.Where(x => x.Id == id).FirstOrDefault();

            this.Musicians.Remove(musicianSelect);            
        }

    }
}
