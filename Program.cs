/* assignment 03
   Lee Burke
   20393941
   Using Memento Design Pattern */
namespace App
{
    public class Program {
        public static void Main(String[] args) {
            Console.Clear();
            var canvas = new List<string>();

            Originator originator = new Originator();
            CareTaker careTaker = new CareTaker();
            
            Boolean exit = false;

            Console.WriteLine("-h           Display this message");
            Console.WriteLine("-a <shape>   Add <circle> | <rectangle>");
            Console.WriteLine("-u           Undo last operation");
            Console.WriteLine("-r           Redo last operation");
            Console.WriteLine("-c           Clear canvas");
            Console.WriteLine("-p           Print current canvas");
            Console.WriteLine("-o           Output canvas as svg file");
            Console.WriteLine("-q           Quit application\n");

            while(exit == false) {
                
                string? input = Console.ReadLine();
                switch(input) {
                    case "-h":
                        Console.WriteLine("\n-h           Display this message");
                        Console.WriteLine("-a <shape>   Add <circle> | <rectangle>");
                        Console.WriteLine("-u           Undo last operation");
                        Console.WriteLine("-r           Redo last operation");
                        Console.WriteLine("-c           Clear canvas");
                        Console.WriteLine("-p           Print current canvas");
                        Console.WriteLine("-o           Output canvas as svg file");
                        Console.WriteLine("-q           Quit application\n");
                    break;
                    case "-a circle":
                        Circle newCirc = new Circle();
                        //Convert circle string to Memento object
                        originator.setShape(newCirc.printShape());
                        //Save Memento object
                        careTaker.add(originator.saveShapeToMemento());
                    break;
                    case "-a rectangle":
                        Rectangle newRect = new Rectangle();
                        //Convert rectangle string to Memento object
                        originator.setShape(newRect.printShape());
                        //Save Memento object
                        careTaker.add(originator.saveShapeToMemento());
                    break;
                    case "-u":
                        careTaker.undo();
                    break;
                    case "-r":
                        careTaker.redo();
                    break;
                    case "-c":
                        careTaker.clear();
                    break;
                    case "-p":
                        for(int i = 0; i < careTaker.getLength(); i++) {
                            originator.getShapeFromMemento(careTaker.get(i));
                            Console.WriteLine(originator.getShape());
                        }
                    break;
                    case "-o":
                        for(int i = 0; i < careTaker.getLength(); i++) {
                            originator.getShapeFromMemento(careTaker.get(i));
                            canvas.Add(originator.getShape());
                        }
                        File.WriteAllLines("canvas.svg", canvas);
                        canvas.Clear();
                    break;
                    case "-q":
                        exit = true;
                    break;
                    default:
                        Console.WriteLine(input + " not recognised. Type -h for help");
                    break;
                }
            }
        }
    }
    public class Circle {
        public int x, y, radius;
        Random r = new Random();
        
        public Circle() {
            this.x = r.Next(0, 500);
            this.y = r.Next(0, 500);
            
            //So that the border of the circle is always contained within the canvas parameters
            
            //if both x && y are >= mid-point
            if(this.x >= 250 && this.y >= 250)
                this.radius = r.Next(1, (this.x > this.y) ? 500 - this.x : 500 - this.y);

            //if both x && y are < mid-point
            else if(this.x < 250 && this.y < 250)
                this.radius = r.Next(1, (this.x < this.y) ? 0 + this.x : 0 + this.y);
                
            //if one value is more and one value is less than mid-point
            else if(this.x < this.y && 0 + this.x <= 500 - this.y)
                this.radius = r.Next(1, 0 + this.x);
            else if(this.x < this.y && 0 + this.x > 500 - this.y)
                this.radius = r.Next(1, 500 - this.y);
            else this.radius = r.Next(1, (500 - this.x < 0 + this.y) ? 500 - this.x : 0 + this.y);
        }
        public string printShape() {
            string data = "<circle cx= \"" + this.x + "\" cy= \"" + this.y +  "\" r= \"" + this.radius + "/>";
            return data;
        }
    }

    public class Rectangle {
        public int x, y, h, w;
        Random r = new Random();
        
        public Rectangle() {
            this.x = r.Next(0, 500);
            this.y = r.Next(0, 500);
            this.h = r.Next(0, 500 - this.y);
            this.w = r.Next(0, 500 - this.x);
        }
        public string printShape() {
            string data = "<rect x= \"" + this.x + "\" y= \"" + this.y +  "\" h= \"" + this.h +  "\" w= \"" + this.w +  "/>";
            return data;
        }
    }

    public class Memento {
        private String shape;

        public Memento(String shape) {
            this.shape = shape;
        }

        public String getShape() {
            return shape;
        }	
    }

    //Originator class converts between strings and Memento objects
    public class Originator { 
        private String? shape;
        public void setShape(String shape){ 
           this.shape = shape;
        }  
        public String getShape(){ 
           return shape; 
        }
        public Memento saveShapeToMemento(){
           return new Memento(shape);
        }  
        public void getShapeFromMemento(Memento memento){ 
            shape = memento.getShape();
        }
    }

    //CareTaker class makes changes to the Memento objects
    public class CareTaker {
        private List<Memento> mementoList = new List<Memento>();
        private List<Memento> undoList = new List<Memento>();

        public void add(Memento shape){
            mementoList.Add(shape);
        }

        public Memento get(int index){
            return mementoList[index];
        }

        public int getLength() {
            return mementoList.Count;
        }

        public Memento undo() {
            if(! mementoList.Any()) return null;
            Memento undoMemento = mementoList[mementoList.Count - 1]; 
            undoList.Add(undoMemento);
            mementoList.Remove(undoMemento);
           return undoMemento;
        }

        public Memento redo() {
            if(! undoList.Any()) return null;
            Memento undoMemento = undoList[undoList.Count - 1];
            mementoList.Add(undoMemento);
            undoList.Remove(undoMemento);
            return undoMemento;
        }

        public void clear() {
            mementoList.Clear();
            undoList.Clear();
        }
    }
}
