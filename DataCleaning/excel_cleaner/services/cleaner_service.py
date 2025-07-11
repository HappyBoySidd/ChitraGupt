from utils import excel_handler as io
from utils import filters

def clean_excel_file(file_path: str):
    """Service layer that orchestrates cleaning tasks."""
    io.close_all_excel_instances()
    df = io.load_excel_file(file_path)

    df = filters.drop_rows_with_exact_hello(df)
    df = filters.remove_word_case_insensitive(df, "into")
    df = filters.remove_word_case_insensitive(df, "hello")
    df = filters.keep_only_ascii_rows(df)
    df = filters.drop_rows_with_question_mark(df)
    df = filters.normalize_translated_fields(df, ['Translated Short Description', 'Translated Description'])

    io.save_to_excel(df, file_path)
    print(f"File cleaned and saved: {file_path}")
