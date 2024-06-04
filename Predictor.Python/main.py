import pandas as pd
import sklearn
raw_frame = pd.read_pickle(R"C:\Users\ianda\source\GitHub\ErmasMachineLearningPredictor\Ignore\PROCESSED_FRAME_UTICA.pkl")
model = pd.read_pickle(R"C:\Users\ianda\source\GitHub\ErmasMachineLearningPredictor\Ignore\GBR_UTICA.pkl")
col_transformer = pd.read_pickle(R"C:\Users\ianda\source\GitHub\ErmasMachineLearningPredictor\Ignore\COL_TRANSFORMER_UTICA.pkl")


transformed = col_transformer.fit_transform(raw_frame)
#print(transformed.head())

predictions = pd.DataFrame(model.predict(transformed.head(n=1)))
print(predictions.head())

#transformed.head(n=1).columns