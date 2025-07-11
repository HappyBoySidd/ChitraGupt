from services.cleaner_service import clean_excel_file

def main():
    file_path = "C:\Datatoclean\Verification.xlsx"
    clean_excel_file(file_path)

if __name__ == "__main__":
    main()
