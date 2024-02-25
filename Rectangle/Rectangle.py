class Rectangle:
    def __init__(self, height, width):
        self.height = height
        self.width = width

    def perimeter(self):
        return 2 * (self.height + self.width)

    def area(self):
        return self.height * self.width

    def print_rectangle(self):
        # Print top row
        print("* " * self.width)
        # Print middle rows with spaces
        for _ in range(self.height - 2):
            print("*" + " " * (self.width * 2 - 3) + "*")
        # Print bottom row 
        print("* " * self.width)

def main():
    while True:
        # Get user input
        print("Rectangle Calculator \n")
        height = int(input("Enter the height of the rectangle: "))
        width = int(input("Enter the width of the rectangle: "))

        # Create a rectangle object
        rectangle = Rectangle(height, width)

        # Print the perimeter, area, and rectangle
        print("\nRectangle Calculator \n")
        print("Height:\t  ", height)
        print("Width:\t  ", width)
        print("Perimeter:", rectangle.perimeter())
        print("Area:\t  ", rectangle.area())
        rectangle.print_rectangle()

        # Ask if the user wants to continue
        continue_choice = input("\nContinue (y/n)? ")
        print()
        if continue_choice.lower() != 'y':
            break

if __name__ == "__main__":
    main() 

