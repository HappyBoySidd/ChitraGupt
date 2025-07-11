import pandas as pd
import win32com.client

def close_all_excel_instances():
    excel = win32com.client.Dispatch("Excel.Application")
    for wb in excel.Workbooks:
        wb.Close(SaveChanges=False)
    excel.Quit()

def load_excel_file(file_path: str) -> pd.DataFrame:
    return pd.read_excel(file_path, engine='openpyxl')

def save_to_excel(df: pd.DataFrame, file_path: str):
    df.to_excel(file_path, index=False, engine='openpyxl')
